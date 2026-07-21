using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Application.Pipelines;

public sealed class SkillNameDataPipeline : IPipeline
{
    public SkillNameDataPipeline(CancellationToken cancellationToken = default)
    {
        var data = new Dictionary<string, string>();
        var input = new ActionBlock<StringNode>(node => data.Add(node.Key, node.Name!),
            new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Input = input;
        Completion = input.Completion;
        Result = CompleteAsync();

        async Task<Dictionary<string, string>> CompleteAsync()
        {
            await Completion.ConfigureAwait(false);
            return data;
        }
    }

    public PipelineId Id => PipelineIds.SkillNameData;
    public ITargetBlock<StringNode> Input { get; }
    public Task<Dictionary<string, string>> Result { get; }
    public Task Completion { get; }
}