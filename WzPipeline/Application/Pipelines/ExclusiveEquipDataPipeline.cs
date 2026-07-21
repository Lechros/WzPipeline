using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Application.Pipelines;

public sealed class ExclusiveEquipDataPipeline : IPipeline
{
    public ExclusiveEquipDataPipeline(ExclusiveEquipParser parser, Task<Dictionary<string, NameDesc>> names,
        CancellationToken cancellationToken = default)
    {
        var data = new SortedDictionary<string, MalibExclusiveEquip>();
        var input = new ActionBlock<ExclusiveEquipNode>(async node =>
        {
            data.Add(node.Id, parser.Parse(node, new ExclusiveEquipParseContext
            {
                GearStringData = await names.ConfigureAwait(false)
            }));
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Input = input;
        Completion = input.Completion;
        Result = CompleteAsync();

        async Task<SortedDictionary<string, MalibExclusiveEquip>> CompleteAsync()
        {
            await Completion.ConfigureAwait(false);
            return data;
        }
    }

    public PipelineId Id => PipelineIds.ExclusiveEquipData;
    public ITargetBlock<ExclusiveEquipNode> Input { get; }
    public Task<SortedDictionary<string, MalibExclusiveEquip>> Result { get; }
    public Task Completion { get; }
}