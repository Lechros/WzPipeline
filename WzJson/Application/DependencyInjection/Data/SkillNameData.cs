using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WzJson.Core.Pipeline;
using WzJson.Core.Pipeline.Runner;
using WzJson.Domains.String;
using WzJson.Shared;
using WzJson.Shared.Processor;
using WzJson.Shared.Traverser;

namespace WzJson.Application.DependencyInjection.Data;

public static class SkillNameData
{
    private const string NodePath = "String/Skill.img/*";

    public static void TryAddSkillNameData(this IServiceCollection services)
    {
        services.TryAddSingleton<NameDescConverter>();
        services.TryAddSingleton(SkillNameDataFactory);
    }

    private static ISkillNameData SkillNameDataFactory(IServiceProvider provider)
    {
        var wzProvider = provider.GetRequiredService<IWzProvider>();
        var traverser = GlobTraverser.Create<INameDescNode>(wzProvider, NodePath, NameDescNode.Create);
        var converter = provider.GetRequiredService<NameDescConverter>();
        var collector = DictionaryCollector.Create((NameDesc nd) => nd.Id, nd => nd.Name!, () => new Domains.String.SkillNameData());

        var pipeline = Builders.LinearPipelineBuilder("SkillNameData")
            .Traverser("Traverse", traverser)
            .Converter("Convert", converter)
            .Processor("Collect", collector)
            .Build();

        var runner = provider.GetRequiredService<IPipelineRunner>();
        var progress = provider.GetService<IProgress<IStepState>>();
        return pipeline.Run(runner, progress).Value;        
    }
}