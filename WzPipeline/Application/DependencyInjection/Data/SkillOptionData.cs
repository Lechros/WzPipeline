using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzPipeline.Core.Pipeline;
using WzPipeline.Core.Pipeline.Runner;
using WzPipeline.OldDomains.Soul.Converters;
using WzPipeline.OldDomains.Soul.Models;
using WzPipeline.OldDomains.Soul.Nodes;
using WzPipeline.OldDomains.Soul.Processors;
using WzPipeline.Shared;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Application.DependencyInjection.Data;

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