using Microsoft.Extensions.DependencyInjection;
using WzPipeline.Core.Pipeline;
using WzPipeline.Core.Pipeline.Runner;
using WzPipeline.Domains.Soul.Converters;
using WzPipeline.Domains.Soul.Models;
using WzPipeline.Domains.Soul.Nodes;
using WzPipeline.Domains.Soul.Processors;
using WzPipeline.Shared;
using WzPipeline.Shared.Processor;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Application.DependencyInjection.Data;

public static class SoulInfoData
{
    private const string NodePath = "Etc/SoulCollection.img/*";

    public static void TryAddSoulInfoData(this IServiceCollection services)
    {
        services.AddSingleton<SoulCollectionConverter>();
        services.AddSingleton<SoulCollectionProcessor>();
        services.AddSingleton(SoulInfoDataFactory);
    }

    private static ISoulInfoData SoulInfoDataFactory(IServiceProvider provider)
    {
        var wzProvider = provider.GetRequiredService<IWzProvider>();
        var traverser =
            GlobTraverser.Create<ISoulCollectionNode>(wzProvider, NodePath, SoulCollectionNodeAdapter.Create);
        var converter = provider.GetRequiredService<SoulCollectionConverter>();
        var processor = provider.GetRequiredService<SoulCollectionProcessor>();
        var collector = DictionaryCollector.Create((SoulInfo si) => si.SoulId, () => new Domains.Soul.Models.SoulInfoData());

        var pipeline = Builders.LinearPipelineBuilder("SoulInfoData")
            .Traverser("Traverse", traverser)
            .Converter("Convert", converter)
            .Processor("Process", processor)
            .Processor("Collect", collector)
            .Build();

        var runner = provider.GetRequiredService<IPipelineRunner>();
        var progress = provider.GetService<IProgress<IStepState>>();
        return pipeline.Run(runner, progress).Value;
    }
}