using System.Diagnostics;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WzJson.Common;
using WzJson.V2.Core.Pipeline;
using WzJson.V2.Domains.Gear.Converters;
using WzJson.V2.Domains.Gear.Models;
using WzJson.V2.Domains.Gear.Nodes;
using WzJson.V2.Domains.Gear.Processors;
using WzJson.V2.Domains.Icon;
using WzJson.V2.Domains.ItemOption;
using WzJson.V2.Domains.String;
using WzJson.V2.Shared.Exporter;
using WzJson.V2.Shared.Json;
using WzJson.V2.Shared.Processor;
using WzJson.V2.Shared.Traverser;

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

        var settings = new JsonSerializerSettings
        {
            Converters = { new PointArrayConverter() },
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
        var itemOptionTraverser =
            new GlobTraverser<IItemOptionNode>(wzProvider, "Item/ItemOption.img/*", ItemOptionNodeAdapter.Create);

        var gearStringData = Builders.LinearPipelineBuilder()
            .Traverser(stringEqpTraverser)
            .Converter(nameDescConverter)
            .Processor(nameDescDictCollector)
            .Build()
            .Run();

        var skillNameData = Builders.LinearPipelineBuilder()
            .Traverser(stringSkillTraverser)
            .Converter(nameDescConverter)
            .Processor(nameDescDictCollector)
            .Build()
            .Run()
            .ToDictionary(e => e.Key, e => e.Value.Name!);

        var itemOptionData = Builders.LinearPipelineBuilder()
            .Traverser(itemOptionTraverser)
            .Converter(new ItemOptionConverter())
            .Processor(new DictionaryCollector<int, ItemOptionEntry>(io => io.Code))
            .Build()
            .Run();

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

        var dictionaryWriter = new DictionaryJsonWriter<string, MalibGear>(jsonSerializer, "gear-data-new.json");
        var iconExporter = new IconImageExporter("output/new-gear-icons");

        var pipeline = Builders.GraphPipelineBuilder()
            .Traverser(gearTraverser, t => t
                .Converter(gearConverter, c => c
                    .Processor(rawGearToMalibGearChain, p => p
                        .Processor(SortedDictionaryCollector.Create((MalibGear g) => g.Meta.Id.ToString()), dc => dc
                            .Exporter(dictionaryWriter))))
                .Converter(iconConverter, c => c
                    .Exporter(iconExporter))
            ).Build();

        pipeline.Run();

        Console.WriteLine(sw.Elapsed);
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