using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzPipeline.Application.DependencyInjection.Data;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Shared;
using WzPipeline.Shared.Exporter;
using WzPipeline.Shared.Processor;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Application.DependencyInjection;

public static class ExclusiveEquip
{
    private const string NodePath = "Etc/ExclusiveEquip.img/*";

    public static void AddExclusiveEquipDataJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddExclusiveEquipDataJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var wzProvider = provider.GetRequiredService<IWzProvider>();
            var traverser = GlobTraverser.Create(wzProvider, NodePath, ExclusiveEquipNodeAdapter.Create);
            var converter = provider.GetRequiredService<ExclusiveEquipConverter>();
            var collector = DictionaryCollector.Create((MalibExclusiveEquip ee) => ee.Id,
                () => new SortedDictionary<int, MalibExclusiveEquip>());
            var exporter = provider.GetRequiredService<DictionaryJsonWriterFactory>()
                .WithFilename<int, MalibExclusiveEquip>(filename);

            ctx.Config
                .Traverser("Traverse ExclusiveEquip", traverser, t => t
                    .Converter("Convert Data", converter, c => c
                        .Processor("Collect", collector, p => p
                            .Exporter("Save Json", exporter))));
        });
    }

    private static void TryAddExclusiveEquipDataJobDependencies(this IServiceCollection services)
    {
        services.TryAddExclusiveEquipConverter();
        services.TryAddDictionaryJsonWriterFactory();
    }

    private static void TryAddExclusiveEquipConverter(this IServiceCollection services)
    {
        services.TryAddGearNameDescData();
        services.TryAddSingleton<ExclusiveEquipConverter>();
    }
}