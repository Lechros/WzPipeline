using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Application.Pipelines;

public sealed class GearStringDataPipeline : IPipeline
{
    public GearStringDataPipeline(CancellationToken cancellationToken = default)
    {
        var data = new Dictionary<string, NameDesc>();
        var input = new ActionBlock<StringNode>(node =>
                data.Add(node.Key, new NameDesc(node.Name, node.Desc)),
            new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Input = input;
        Completion = input.Completion;
        Result = CompleteAsync();

        async Task<Dictionary<string, NameDesc>> CompleteAsync()
        {
            await Completion.ConfigureAwait(false);
            return data;
        }
    }

    public PipelineId Id => PipelineIds.GearStringData;
    public ITargetBlock<StringNode> Input { get; }
    public Task<Dictionary<string, NameDesc>> Result { get; }
    public Task Completion { get; }
}