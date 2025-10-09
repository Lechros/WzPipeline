using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Core.Pipeline.Graph;
using WzJson.Domains.Icon;
using WzJson.Shared;
using WzJson.Shared.Exporter;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;
using IconConverter = WzJson.Domains.Icon.IconConverter;

namespace WzJson.Application.DependencyInjection;

public static class Item
{
    private const string NodePath = "Item/{Cash,Consume,Etc}/*.img/*";

    private const string TraverserConfigKey = "ItemTraverser";
    private const string IconConverterConfigKey = "ItemIconConverter";
    private const string RawIconConverterConfigKey = "ItemRawIconConverter";

    public static void AddItemIconJob(this Workflow workflow, string outputPath)
    {
        workflow.ServiceCollection.TryAddItemIconJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var exporter = new IconImageExporter(outputPath);

            IconConverter(provider, ctx)
                .Exporter("Save Icon Png", exporter);
        });
    }

    private static void TryAddItemIconJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
    }

    public static void AddItemIconOriginJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddItemIconOriginJobDependencies();

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

    private static void TryAddItemIconOriginJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
        services.TryAddSingleton<NaturalStringComparer>();
        services.TryAddDictionaryJsonWriterFactory();
    }

    public static void AddItemRawIconJob(this Workflow workflow, string outputPath)
    {
        workflow.ServiceCollection.TryAddItemRawIconJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var exporter = new IconImageExporter(outputPath);

            RawIconConverter(provider, ctx)
                .Exporter("Save Raw Icon Png", exporter);
        });
    }

    private static void TryAddItemRawIconJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
    }

    public static void AddItemRawIconOriginJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddItemRawIconOriginJobDependencies();

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

    private static void TryAddItemRawIconOriginJobDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IconConverter>();
        services.TryAddSingleton<NaturalStringComparer>();
        services.TryAddDictionaryJsonWriterFactory();
    }

    private static TraverserConfig<DefaultIconNodeAdapter> ItemTraverser(IServiceProvider provider,
        ConfigContext ctx)
    {
        return ctx.GetConfigOrCreate(TraverserConfigKey, () =>
        {
            TraverserConfig<DefaultIconNodeAdapter>? config = null;
            var wzProvider = provider.GetRequiredService<IWzProvider>();
            var traverser = GlobTraverser.Create(wzProvider, NodePath,
                node => DefaultIconNodeAdapter.Create(node, wzProvider.FindNode));
            ctx.Config
                .Traverser("Traverse Item", traverser, s => config = s);
            return config!;
        });
    }

    private static ConverterConfig<DefaultIconNodeAdapter, IconOrigin> IconConverter(IServiceProvider provider,
        ConfigContext ctx)
    {
        return ctx.GetConfigOrCreate(IconConverterConfigKey, () =>
        {
            ConverterConfig<DefaultIconNodeAdapter, IconOrigin>? config = null;
            var converter = provider.GetRequiredService<IconConverter>();
            ItemTraverser(provider, ctx)
                .Converter("Convert Icon", converter, c => config = c);
            return config!;
        });
    }

    private static ConverterConfig<DefaultIconNodeAdapter, IconOrigin> RawIconConverter(IServiceProvider provider,
        ConfigContext ctx)
    {
        return ctx.GetConfigOrCreate(RawIconConverterConfigKey, () =>
        {
            ConverterConfig<DefaultIconNodeAdapter, IconOrigin>? config = null;
            var converter = provider.GetRequiredService<IconConverter>();
            ItemTraverser(provider, ctx)
                .Converter("Convert Raw Icon", converter, c => config = c);
            return config!;
        });
    }
}