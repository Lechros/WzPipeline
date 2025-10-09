using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Runner;
using WzJson.Domains.String;
using WzJson.Shared;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;

namespace WzJson.Application.DependencyInjection.Data;

public static class ConsumeNameData
{
    private const string NodePath = "String/Consume.img/*";

    public static void TryAddConsumeNameData(this IServiceCollection services)
    {
        services.TryAddSingleton<NameDescConverter>();
        services.TryAddSingleton(ConsumeNameDataFactory);
    }

    private static IConsumeNameData ConsumeNameDataFactory(IServiceProvider provider)
    {
        var wzProvider = provider.GetRequiredService<IWzProvider>();
        var traverser = GlobTraverser.Create<INameDescNode>(wzProvider, NodePath, NameDescNode.Create);
        var converter = provider.GetRequiredService<NameDescConverter>();
        var collector = DictionaryCollector.Create((NameDesc nd) => nd.Id, nd => nd.Name!, () => new Domains.String.ConsumeNameData());

        var pipeline = Builders.LinearPipelineBuilder("ConsumeNameData")
            .Traverser("Traverse", traverser)
            .Converter("Convert", converter)
            .Processor("Collect", collector)
            .Build();

        var runner = provider.GetRequiredService<IPipelineRunner>();
        var progress = provider.GetService<IProgress<IStepState>>();
        return pipeline.Run(runner, progress).Value;
    }
}