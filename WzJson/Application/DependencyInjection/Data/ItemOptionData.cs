using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Runner;
using WzJson.Domains.ItemOption;
using WzJson.Shared;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;

namespace WzJson.Application.DependencyInjection.Data;

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