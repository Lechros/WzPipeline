using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzPipeline.Core.Pipeline;
using WzPipeline.Core.Pipeline.Runner;
using WzPipeline.Domains.SubWeaponTransfer;
using WzPipeline.Shared;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Application.DependencyInjection.Data;

public static class AstraSubWeaponData
{
    private const string NodePath = "Etc/SubWeaponTransferData.img/Job/*";

    public static void TryAddAstraSubWeaponData(this IServiceCollection services)
    {
        services.TryAddSingleton<AstraSubWeaponConverter>();
        services.TryAddSingleton<AstraSubWeaponCollector>();
        services.TryAddSingleton(AstraSubWeaponDataFactory);
    }

    private static IAstraSubWeaponData AstraSubWeaponDataFactory(IServiceProvider provider)
    {
        var wzProvider = provider.GetRequiredService<IWzProvider>();
        var traverser = GlobTraverser.Create<ISubWeaponTransferNode>(wzProvider, NodePath, SubWeaponTransferNodeAdapter.Create);
        var converter = provider.GetRequiredService<AstraSubWeaponConverter>();
        var collector = provider.GetRequiredService<AstraSubWeaponCollector>();

        var pipeline = Builders.LinearPipelineBuilder("AstraSubWeaponData")
            .Traverser("Traverse", traverser)
            .Converter("Convert", converter)
            .Processor("Collect", collector)
            .Build();

        var runner = provider.GetRequiredService<IPipelineRunner>();
        var progress = provider.GetService<IProgress<IStepState>>();
        return pipeline.Run(runner, progress).Value;

    }
}