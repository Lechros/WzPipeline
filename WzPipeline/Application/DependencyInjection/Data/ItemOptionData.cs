using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzPipeline.Core.Pipeline;
using WzPipeline.Core.Pipeline.Runner;
using WzPipeline.Domains.ItemOption;
using WzPipeline.Shared;
using WzPipeline.Shared.Processor;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Application.DependencyInjection.Data;

public static class ItemOptionData
{
    private const string NodePath = "Item/ItemOption.img/*";

    public static void TryAddItemOptionData(this IServiceCollection services)
    {
        services.TryAddSingleton<ItemOptionConverter>();
        services.TryAddSingleton(ItemOptionDataFactory);
    }

    private static IItemOptionData ItemOptionDataFactory(IServiceProvider provider)
    {
        var wzProvider = provider.GetRequiredService<IWzProvider>();
        var traverser = GlobTraverser.Create<IItemOptionNode>(wzProvider, NodePath, ItemOptionNodeAdapter.Create);
        var converter = provider.GetRequiredService<ItemOptionConverter>();
        var collector = DictionaryCollector.Create((ItemOptionEntry io) => io.Code, () => new Domains.ItemOption.ItemOptionData());

        var pipeline = Builders.LinearPipelineBuilder("ItemOptionData")
            .Traverser("Traverse", traverser)
            .Converter("Convert", converter)
            .Processor("Collect", collector)
            .Build();

        var runner = provider.GetRequiredService<IPipelineRunner>();
        var progress = provider.GetService<IProgress<IStepState>>();
        return pipeline.Run(runner, progress).Value;
    }
}