using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzPipeline.Application.DependencyInjection.Data;
using WzPipeline.Domains.SetItem;
using WzPipeline.Shared;
using WzPipeline.Shared.Exporter;
using WzPipeline.Shared.Processor;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Application.DependencyInjection;

public static class SetItem
{
    private const string NodePath = "Etc/SetItemInfo.img/*";

    public static void AddSetItemDataJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddSetItemDataJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var wzProvider = provider.GetRequiredService<IWzProvider>();
            var traverser = GlobTraverser.Create<ISetItemNode>(wzProvider, NodePath, SetItemNodeAdapter.Create);
            var converter = provider.GetRequiredService<MalibSetItemConverter>();
            var collector = DictionaryCollector.Create((MalibSetItem s) => s.Id,
                () => new SortedDictionary<int, MalibSetItem>());
            var exporter = provider.GetRequiredService<DictionaryJsonWriterFactory>()
                .WithFilename<int, MalibSetItem>(filename);

            ctx.Config
                .Traverser("Traverse SetItem", traverser, t => t
                    .Converter("Convert Data", converter, c => c
                        .Processor("Collect", collector, p => p
                            .Exporter("Save Json", exporter))));
        });
    }

    private static void TryAddSetItemDataJobDependencies(this IServiceCollection services)
    {
        services.TryAddMalibSetItemConverter();
        services.TryAddDictionaryJsonWriterFactory();
    }

    private static void TryAddMalibSetItemConverter(this IServiceCollection services)
    {
        services.TryAddItemOptionData();
        services.TryAddSingleton<MalibSetItemConverter>();
    }
}