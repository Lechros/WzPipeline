using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Runner;
using WzJson.Domains.Soul.Converters;
using WzJson.Domains.Soul.Models;
using WzJson.Domains.Soul.Nodes;
using WzJson.Domains.Soul.Processors;
using WzJson.Shared;
using WzJson.Shared.Traverser;

namespace WzJson.Application.DependencyInjection.Data;

public static class SkillOptionData
{
    private const string NodePath = "Item/SkillOption.img/skill/*";

    public static void TryAddSkillOptionData(this IServiceCollection services)
    {
        services.TryAddSkillOptionConverter();
        services.TryAddSingleton<SkillOptionCollector>();
        services.TryAddSingleton(SkillOptionDataFactory);
    }

    private static ISkillOptionData SkillOptionDataFactory(IServiceProvider provider)
    {
        var wzProvider = provider.GetRequiredService<IWzProvider>();
        var traverser = GlobTraverser.Create<ISkillOptionNode>(wzProvider, NodePath, SkillOptionNodeAdapter.Create);
        var converter = provider.GetRequiredService<SkillOptionConverter>();
        var collector = provider.GetRequiredService<SkillOptionCollector>();

        var pipeline = Builders.LinearPipelineBuilder("SkillOptionData")
            .Traverser("Traverse", traverser)
            .Converter("Convert", converter)
            .Processor("Collect", collector)
            .Build();

        var runner = provider.GetRequiredService<IPipelineRunner>();
        var progress = provider.GetService<IProgress<IStepState>>();
        return pipeline.Run(runner, progress).Value;
    }

    private static void TryAddSkillOptionConverter(this IServiceCollection services)
    {
        services.TryAddItemOptionData();
        services.TryAddSingleton<SkillOptionConverter>();
    }
}