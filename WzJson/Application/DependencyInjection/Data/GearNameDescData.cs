using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Runner;
using WzJson.Domains.Gear.Models;
using WzJson.Domains.String;
using WzJson.Shared;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;

namespace WzJson.Application.DependencyInjection.Data;

public static class GearNameDescData
{
    private const string NodePath = "String/Eqp.img/Eqp/*/*";

    public static void TryAddGearNameDescData(this IServiceCollection services)
    {
        services.TryAddSingleton<NameDescConverter>();
        services.TryAddSingleton(GearNameDescDataFactory);
    }

    private static IGearNameDescData GearNameDescDataFactory(IServiceProvider provider)
    {
        var wzProvider = provider.GetRequiredService<IWzProvider>();
        var traverser = GlobTraverser.Create<INameDescNode>(wzProvider, NodePath, NameDescNode.Create);
        var converter = provider.GetRequiredService<NameDescConverter>();
        var collector = DictionaryCollector.Create((NameDesc nd) => nd.Id, () => new Domains.Gear.Models.GearNameDescData());

        var pipeline = Builders.LinearPipelineBuilder("GearNameDescData")
            .Traverser("Traverse", traverser)
            .Converter("Convert", converter)
            .Processor("Collect", collector)
            .Build();

        var runner = provider.GetRequiredService<IPipelineRunner>();
        var progress = provider.GetService<IProgress<IStepState>>();
        return pipeline.Run(runner, progress).Value;
    }
}