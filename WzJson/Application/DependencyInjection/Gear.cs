using System.Drawing;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Application.DependencyInjection.Data;
using WzJson.Core.Pipeline.Graph;
using WzJson.Domains.Gear.Converters;
using WzJson.Domains.Gear.Models;
using WzJson.Domains.Gear.Nodes;
using WzJson.Domains.Gear.Processors;
using WzJson.Domains.Icon;
using WzJson.Shared;
using WzJson.Shared.Exporter;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;
using IconConverter = WzJson.Domains.Icon.IconConverter;

namespace WzJson.Application.DependencyInjection;

public static class Gear
{
    private const string NodePath =
        "Character/{Accessory,Android,Cap,Cape,Coat,Dragon,Glove,Longcoat,Mechanic,Pants,Ring,Shield,Shoes,Weapon}/*.img";

    private const string TraverserConfigKey = "GearTraverser";
    private const string IconConverterConfigKey = "GearIconConverter";
    private const string RawIconConverterConfigKey = "GearRawIconConverter";

    public static void AddGearDataJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddGearDataJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var converter = provider.GetRequiredService<RawGearConverter>();
            var processor1 = provider.GetRequiredService<RawGearToGearProcessor>();
            var processor2 = provider.GetRequiredService<GearRawAttributesProcessor>();
            var processor3 = provider.GetRequiredService<GearSkillsProcessor>();
            var collector = DictionaryCollector.Create((MalibGear g) => g.Meta.Id,
                () => new SortedDictionary<int, MalibGear>());
            var exporter = provider.GetRequiredService<DictionaryJsonWriterFactory>()
                .WithFilename<int, MalibGear>(filename);

            GearTraverser(provider, ctx)
                .Converter("Convert Data", converter, c => c
                    .Processor("Process RawGearToGear", processor1, p1 => p1
                        .Processor("Process GearRawAttributes", processor2, p2 => p2
                            .Processor("Process GearSkills", processor3, p3 => p3
                                .Processor("Collect", collector, p4 => p4
                                    .Exporter("Save Json", exporter))))));
        });
    }

    private static void TryAddGearDataJobDependencies(this IServiceCollection services)
    {
        services.TryAddRawGearConverter();
        services.TryAddSingleton<RawGearToGearProcessor>();
        services.TryAddGearRawAttributesProcessor();
        services.TryAddGearSkillsProcessor();
        services.TryAddDictionaryJsonWriterFactory();
    }

    public static void AddGearIconJob(this Workflow workflow, string outputPath)
    {
        workflow.ServiceCollection.TryAddGearIconJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var exporter = new IconImageExporter(outputPath);

            IconConverter(provider, ctx)
                .Exporter("Save Icon Png", exporter);
        });
    }

    private static void TryAddGearIconJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
    }

    public static void AddGearIconOriginJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddGearIconOriginJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var comparer = provider.GetRequiredService<NaturalStringComparer>();
            var collector = DictionaryCollector.Create((IconOrigin io) => io.Id, io => io.Origin,
                () => new SortedDictionary<string, Point>(comparer));
            var exporter = provider.GetRequiredService<DictionaryJsonWriterFactory>()
                .WithFilename<string, Point>(filename);

            IconConverter(provider, ctx)
                .Processor("Collect", collector, p => p
                    .Exporter("Save Origin Json", exporter));
        });
    }

    private static void TryAddGearIconOriginJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
        services.TryAddSingleton<NaturalStringComparer>();
        services.TryAddDictionaryJsonWriterFactory();
    }

    public static void AddGearRawIconJob(this Workflow workflow, string outputPath)
    {
        workflow.ServiceCollection.TryAddGearRawIconJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var exporter = new IconImageExporter(outputPath);

            RawIconConverter(provider, ctx)
                .Exporter("Save Raw Icon Png", exporter);
        });
    }

    private static void TryAddGearRawIconJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
    }

    public static void AddGearRawIconOriginJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddGearRawIconOriginJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var comparer = provider.GetRequiredService<NaturalStringComparer>();
            var collector = DictionaryCollector.Create((IconOrigin io) => io.Id, io => io.Origin,
                () => new SortedDictionary<string, Point>(comparer));
            var exporter = provider.GetRequiredService<DictionaryJsonWriterFactory>()
                .WithFilename<string, Point>(filename);

            RawIconConverter(provider, ctx)
                .Processor("Collect", collector, p => p
                    .Exporter("Save Raw Origin Json", exporter));
        });
    }

    private static void TryAddGearRawIconOriginJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
        services.TryAddSingleton<NaturalStringComparer>();
        services.TryAddDictionaryJsonWriterFactory();
    }

    private static void TryAddRawGearConverter(this IServiceCollection services)
    {
        services.TryAddGearNameDescData();
        services.TryAddItemOptionData();
        services.TryAddSingleton<RawGearConverter>();
    }

    private static void TryAddGearRawAttributesProcessor(this IServiceCollection services)
    {
        services.AddNodeJS();
        services.AddSingleton<IJsonService, JsonService>();
        services.TryAddSingleton(GearRawAttributesProcessorFactory);
    }

    private static GearRawAttributesProcessor GearRawAttributesProcessorFactory(IServiceProvider provider)
    {
        var nodeJsService = provider.GetRequiredService<INodeJSService>();
        return new GearRawAttributesProcessor(nodeJsService, "Scripts/dist/index.js");
    }

    private static void TryAddGearSkillsProcessor(this IServiceCollection services)
    {
        services.TryAddSkillNameData();
        services.TryAddSingleton<GearSkillsProcessor>();
    }

    private static TraverserConfig<IGearNode> GearTraverser(IServiceProvider provider, ConfigContext ctx)
    {
        return ctx.GetConfigOrCreate(TraverserConfigKey, () =>
        {
            TraverserConfig<IGearNode>? config = null;
            var wzProvider = provider.GetRequiredService<IWzProvider>();
            var traverser = GlobTraverser.Create<IGearNode>(wzProvider, NodePath,
                node => GearNodeAdapter.Create(node, wzProvider.FindNode));
            ctx.Config
                .Traverser("Traverse Gear", traverser, s => config = s);
            return config!;
        });
    }

    private static ConverterConfig<IGearNode, IconOrigin> IconConverter(IServiceProvider provider,
        ConfigContext ctx)
    {
        return ctx.GetConfigOrCreate(IconConverterConfigKey, () =>
        {
            ConverterConfig<IGearNode, IconOrigin>? config = null;
            var converter = provider.GetRequiredService<IconConverter>();
            GearTraverser(provider, ctx)
                .Converter("Convert Icon", converter, c => config = c);
            return config!;
        });
    }

    private static ConverterConfig<IGearNode, IconOrigin> RawIconConverter(IServiceProvider provider,
        ConfigContext ctx)
    {
        return ctx.GetConfigOrCreate(RawIconConverterConfigKey, () =>
        {
            ConverterConfig<IGearNode, IconOrigin>? config = null;
            var converter = provider.GetRequiredService<IconConverter>();
            GearTraverser(provider, ctx)
                .Converter("Convert Raw Icon", converter, c => config = c);
            return config!;
        });
    }
}