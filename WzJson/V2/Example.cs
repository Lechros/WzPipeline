using System.Diagnostics;
using System.Text;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Spectre.Console;
using WzJson.V2.Core.Pipeline;
using WzJson.V2.Core.Pipeline.Runner;
using WzJson.V2.Domains.Gear.Converters;
using WzJson.V2.Domains.Gear.Models;
using WzJson.V2.Domains.Gear.Nodes;
using WzJson.V2.Domains.Gear.Processors;
using WzJson.V2.Domains.Icon;
using WzJson.V2.Domains.ItemOption;
using WzJson.V2.Domains.SetItem;
using WzJson.V2.Domains.Soul.Converters;
using WzJson.V2.Domains.Soul.Models;
using WzJson.V2.Domains.Soul.Processors;
using WzJson.V2.Domains.Soul.Traversers;
using WzJson.V2.Domains.String;
using WzJson.V2.Shared;
using WzJson.V2.Shared.Exporter;
using WzJson.V2.Shared.Json;
using WzJson.V2.Shared.Processor;
using WzJson.V2.Shared.Traverser;
using ItemOptionConverter = WzJson.V2.Domains.ItemOption.ItemOptionConverter;
using SkillOptionConverter = WzJson.V2.Domains.Soul.Converters.SkillOptionConverter;

namespace WzJson.V2;

public class Example
{
    /**
     * 1. 굳이 Exporter를 만들 필요는 없다. Processor의 일종으로 볼 수 있다.
     * 2. Exporter의 path도 필요 없다.
     */
    public static void Main(string[] args)
    {
        var sw = Stopwatch.StartNew();

        Console.WriteLine(Directory.GetCurrentDirectory());

        var settings = new JsonSerializerSettings
        {
            Converters = { new PointArrayConverter() }, // Use DI
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        var wzProvider = new WzProvider(@"C:\Nexon\Maple\Data\Base\Base.wz");

        var nameDescConverter = new NameDescConverter();
        var nameDescDictCollector = new DictionaryCollector<string, NameDesc>(nd => nd.Id);

        var services = new ServiceCollection();

        services.AddNodeJS();
        services.AddSingleton<JsonSerializer>(_ => JsonSerializer.Create(settings));
        services.AddSingleton<IJsonService, JsonService>();

        var serviceProvider = services.BuildServiceProvider();
        var nodeJsService = serviceProvider.GetRequiredService<INodeJSService>();

        var stringEqpTraverser = new GlobTraverser<INameDescNode>(wzProvider,
            "String/Eqp.img/Eqp/{Accessory,Android,Cap,Cape,Coat,Dragon,Glove,Longcoat,Mechanic,Pants,Ring,Shield,Shoes,Weapon}/*",
            NameDescNode.Create);
        var stringSkillTraverser =
            new GlobTraverser<INameDescNode>(wzProvider, "String/Skill.img/*", NameDescNode.Create);
        var stringConsumeTraverser = new GlobTraverser<INameDescNode>(wzProvider,
            "String/Consume.img/*", NameDescNode.Create);
        var itemOptionTraverser =
            new GlobTraverser<IItemOptionNode>(wzProvider, "Item/ItemOption.img/*", ItemOptionNodeAdapter.Create);
        var soulCollectionTraverser = new SoulCollectionNodeTraverser(wzProvider);
        var soulCollectionConverter = new SoulCollectionConverter();
        var soulCollectionProcessor = new SoulCollectionProcessor();

        EventHandler<IStepState> GraphHandlerGenerator(LiveDisplayContext ctx)
        {
            var nextTime = DateTime.UtcNow;
            return (sender, value) =>
            {
                var now = DateTime.UtcNow;
                if (nextTime > now) return;
                nextTime = now.AddMilliseconds(20);

                ctx.UpdateTarget(BuildTree(value));
            };
        }

        AnsiConsole.Live(new Text("Loading...")).Start(ctx =>
        {
            var handler = GraphHandlerGenerator(ctx);
            var progress = new Progress<IStepState>();
            progress.ProgressChanged += handler;

            var gearStringData = Builders.LinearPipelineBuilder("Parse gear string data")
                .Traverser("String/Eqp", stringEqpTraverser)
                .Converter("Convert to NameDesc", nameDescConverter)
                .Processor("Collect to Dictionary", nameDescDictCollector)
                .Build()
                .Run(progress).Value;

            var skillNameData = Builders.LinearPipelineBuilder("Parse skill name data")
                .Traverser("String/Skill", stringSkillTraverser)
                .Converter("Convert to NameDesc", nameDescConverter)
                .Processor("Collect to Dictionary", nameDescDictCollector)
                .Build()
                .Run(progress).Value
                .ToDictionary(e => e.Key, e => e.Value.Name!);

            var itemOptionData = Builders.LinearPipelineBuilder("Parse item option data")
                .Traverser("ItemOption", itemOptionTraverser)
                .Converter("Convert to ItemOption", new ItemOptionConverter())
                .Processor("Collect to Dictionary", new DictionaryCollector<int, ItemOptionEntry>(io => io.Code))
                .Build()
                .Run(progress).Value;

            var skillOptionData = Builders.LinearPipelineBuilder("Parse skill option data")
                .Traverser("SkillOption", new SkillOptionNodeTraverser(wzProvider))
                .Converter("Convert to SkillOption", new SkillOptionConverter(itemOptionData))
                .Processor("Collect to Dictionary", new SkillOptionCollector())
                .Build()
                .Run(progress).Value;

            var consumeNameData = Builders.LinearPipelineBuilder("Parse consume name data")
                .Traverser("String/Consume", stringConsumeTraverser)
                .Converter("Convert to NameDesc", nameDescConverter)
                .Processor("Collect to Dictionary", nameDescDictCollector)
                .Build()
                .Run(progress).Value
                .ToDictionary(e => e.Key, e => e.Value.Name!);

            var soulInfoData = Builders.LinearPipelineBuilder("Parse soul info data")
                .Traverser("SoulCollection", soulCollectionTraverser)
                .Converter("Convert to SoulCollection", soulCollectionConverter)
                .Processor("Process to SoulInfo", soulCollectionProcessor)
                .Processor("Collect to Dictionary", new DictionaryCollector<int, SoulInfo>(io => io.SoulId))
                .Build()
                .Run(progress).Value;

            var gearTraverser = new GlobTraverser<IGearNode>(
                wzProvider,
                "Character/{Accessory,Android,Cap,Cape,Coat,Dragon,Glove,Longcoat,Mechanic,Pants,Ring,Shield,Shoes,Weapon}/*.img",
                (node) => GearNodeAdapter.Create(node, wzProvider.FindNode));
            var gearConverter = new RawGearConverter(gearStringData, itemOptionData);
            var rawGearToGearProcessor = new RawGearToGearProcessor();
            var gearRawAttributesProcessor = new GearRawAttributesProcessor(nodeJsService, "Scripts/dist/index.js");
            var gearSkillsAddingProcessor = new GearSkillsProcessor(skillNameData);
            var rawGearToMalibGearChain =
                ProcessorChain.Of(rawGearToGearProcessor, gearRawAttributesProcessor, gearSkillsAddingProcessor);
            var jsonSerializer = serviceProvider.GetRequiredService<JsonSerializer>();
            var iconConverter = new IconConverter();
            var rawIconConverter = new RawIconConverter();

            var gearDictionaryWriter =
                new DictionaryJsonWriter<string, MalibGear>(jsonSerializer, "gear-data-new.json");
            var gearIconExporter = new IconImageExporter("output/new-gear-icons");
            var soulDictionaryWriter =
                new DictionaryJsonWriter<int, MalibSoul>(jsonSerializer, "output/soul-data-new.json");

            var soulTraverser = new SoulNodeTraverser(wzProvider);
            var soulConverter = new MalibSoulConverter(consumeNameData, skillNameData, soulInfoData, skillOptionData);

            var setItemTraverser =
                new GlobTraverser<ISetItemNode>(wzProvider, "Etc/SetItemInfo.img/*", SetItemNodeAdapter.Create);
            var setItemConverter = new MalibSetItemConverter(itemOptionData);
            var setItemCollector = SortedDictionaryCollector.Create((MalibSetItem s) => s.Id);
            var setItemDictWriter =
                new DictionaryJsonWriter<int, MalibSetItem>(jsonSerializer, "output/set-item-data-new.json");

            var itemTraverser = new GlobTraverser<DefaultIconNodeAdapter>(wzProvider, "Item/{Cash,Consume,Etc}/*.img/*",
                (node) => DefaultIconNodeAdapter.Create(node, wzProvider.FindNode));
            var itemIconExporter = new IconImageExporter("output/new-item-icons");
            var itemRawIconExporter = new IconImageExporter("output/new-item-raw-icons");

            var result = Builders.GraphPipelineBuilder("Gear export pipeline")
                // .Traverser("Gears", gearTraverser, t => t
                //     .Converter("Convert to RawGear", gearConverter, c => c
                //         .Processor("Convert to MalibGear", rawGearToMalibGearChain, p => p
                //             .Processor("Collect to SortedDictionary",
                //                 SortedDictionaryCollector.Create((MalibGear g) => g.Meta.Id.ToString()), dc => dc
                //                     .Exporter("Write JSON file", gearDictionaryWriter))))
                //     .Converter("Extract Icon", iconConverter, c => c
                //         .Exporter("Save PNG files", gearIconExporter)))
                .Traverser("Souls", soulTraverser, t => t
                    .Converter("Convert to MalibSoul", soulConverter, c => c
                        .Processor("Collect to SortedDictionary",
                            SortedDictionaryCollector.Create((MalibSoul s) => s.Id), e => e
                                .Exporter("Write JSON file", soulDictionaryWriter))))
                .Traverser("SetItem", setItemTraverser, t => t
                    .Converter("Convert to MalibSetItem", setItemConverter, c => c
                        .Processor("Collect to SortedDictionary", setItemCollector, p => p
                            .Exporter("Write JSON file", setItemDictWriter))))
                .Traverser("Item", itemTraverser, t => t
                    .Converter("Convert to Icon", iconConverter, c => c
                        .Exporter("Save PNG files", itemIconExporter))
                    .Converter("Convert to Raw Icon", rawIconConverter, c => c
                        .Exporter("Save PNG files", itemRawIconExporter)))
                .Build()
                .Run(progress);

            progress.ProgressChanged -= handler;
            ctx.UpdateTarget(BuildTree(result.State));
            ctx.Refresh();
        });
    }

    private static Tree BuildTree(IStepState state)
    {
        var tree = new Tree(NodeStateToString(state)).Guide(TreeGuide.Line);
        foreach (var child in state.Children)
        {
            tree.AddNode(BuildTree(child));
        }

        return tree;
    }

    private static string NodeStateToString(IStepState state)
    {
        var PROGRESS_LENGTH = 20;

        var sb = new StringBuilder();
        if (state.Status == NodeStatus.Pending)
            sb.Append($"[grey]Pending[/]");
        if (state.Status == NodeStatus.Running)
            sb.Append($"[yellow]Running[/]");
        if (state.Status == NodeStatus.Complete)
            sb.Append($"[green]Complete[/]");

        sb.Append(' ');
        sb.Append(state.Name);

        if (state.Status == NodeStatus.Running && state.TotalCount != null)
        {
            sb.Append("   [[");
            var done = (int)Math.Round((double)PROGRESS_LENGTH * state.Count / state.TotalCount.Value);
            for (int i = 0; i < done; i++)
                sb.Append('=');
            for (int i = done; i < PROGRESS_LENGTH; i++)
                sb.Append(' ');
            sb.Append("]]");
        }

        sb.Append(' ');
        sb.Append(state.Count);
        sb.Append('/');
        if (state.TotalCount != null)
            sb.Append(state.TotalCount);
        else
            sb.Append('?');

        sb.Append($" [blue]({state.Duration.TotalSeconds:0.00}s)[/]");
        return sb.ToString();
    }
}

internal class JsonService(JsonSerializer serializer) : IJsonService
{
    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
    {
        return new ValueTask<T?>(Task.Run(() =>
        {
            using var sr = new StreamReader(stream, leaveOpen: true);
            using var jsonReader = new JsonTextReader(sr);
            return serializer.Deserialize<T>(jsonReader);
        }, cancellationToken));
    }

    public Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            using var sw = new StreamWriter(stream, leaveOpen: true);
            using var jsonWriter = new JsonTextWriter(sw);
            jsonWriter.Formatting = serializer.Formatting;
            serializer.Serialize(jsonWriter, value);
            sw.Flush();
        }, cancellationToken);
    }
}