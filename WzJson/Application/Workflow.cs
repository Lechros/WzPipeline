using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Core.Pipeline.Graph;
using WzJson.Core.Pipeline.Runner;
using WzJson.Shared;

namespace WzJson.Application;

public class Workflow
{
    public IServiceCollection ServiceCollection { get; }
    
    internal readonly List<ConfigureJobFunction> Configurers = [];

    public Workflow(IServiceCollection serviceCollection, IWzProvider wzProvider)
    {
        ServiceCollection = serviceCollection;
        ServiceCollection.AddSingleton(wzProvider);
        ServiceCollection.TryAddSingleton<IPipelineRunner, DefaultPipelineRunner>();
    }

    public void Run(IServiceProvider serviceProvider)
    {
        var pipeline = Build(serviceProvider);

        var runner = serviceProvider.GetRequiredService<IPipelineRunner>();
        var progress = serviceProvider.GetService<IProgress<IStepState>>();
        pipeline.Run(runner, progress);
    }

    private GraphPipeline Build(IServiceProvider serviceProvider)
    {
        var ctx = new ConfigContext("App");
        foreach (var builder in Configurers)
        {
            builder(serviceProvider, ctx);
        }

        return ctx.Config.Build();
    }
}