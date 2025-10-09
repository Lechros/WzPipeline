using Microsoft.Extensions.DependencyInjection;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Runner;
using WzJson.Domains.Soul.Converters;
using WzJson.Domains.Soul.Models;
using WzJson.Domains.Soul.Nodes;
using WzJson.Domains.Soul.Processors;
using WzJson.Shared;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;

namespace WzJson.Application.DependencyInjection.Data;

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