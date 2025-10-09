using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Application.DependencyInjection.Data;
using WzJson.Domains.Soul.Converters;
using WzJson.Domains.Soul.Models;
using WzJson.Domains.Soul.Nodes;
using WzJson.Shared;
using WzJson.Shared.Exporter;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;

namespace WzJson.Application.DependencyInjection;

public static class Soul
{
    private const string NodePath = "Item/Consume/0259.img/*";

    public static void AddSoulDataJob(this Workflow workflow, string filename)
    {
        workflow.ServiceCollection.TryAddSoulDataJobDependencies();

        workflow.Configurers.Add((provider, ctx) =>
        {
            var wzProvider = provider.GetRequiredService<IWzProvider>();
            var traverser = GlobTraverser.Create(wzProvider, NodePath, SoulNodeAdapter.Create);
            var converter = provider.GetRequiredService<MalibSoulConverter>();
            var collector = DictionaryCollector.Create((MalibSoul g) => g.Id,
                () => new SortedDictionary<int, MalibSoul>());
            var exporter = provider.GetRequiredService<DictionaryJsonWriterFactory>()
                .WithFilename<int, MalibSoul>(filename);

            ctx.Config
                .Traverser("Traverse Soul", traverser, t => t
                    .Converter("Convert Data", converter, c => c
                        .Processor("Collect", collector, p => p
                            .Exporter("Save Json", exporter))));
        });
    }

    private static void TryAddSoulDataJobDependencies(this IServiceCollection services)
    {
        services.TryAddMalibSoulConverter();
        services.TryAddDictionaryJsonWriterFactory();
    }

    private static void TryAddMalibSoulConverter(this IServiceCollection services)
    {
        services.TryAddConsumeNameData();
        services.TryAddSkillNameData();
        services.TryAddSoulInfoData();
        services.TryAddSkillOptionData();
        services.TryAddSingleton<MalibSoulConverter>();
    }
}