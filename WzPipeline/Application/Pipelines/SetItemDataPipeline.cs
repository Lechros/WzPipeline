using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Domains.SetItem;
using WzPipeline.Domains.Shared.ItemOption;

namespace WzPipeline.Application.Pipelines;

public sealed class SetItemDataPipeline : IPipeline
{
    public SetItemDataPipeline(SetItemParser parser, Task<ItemOptionData> itemOptions,
        CancellationToken cancellationToken = default)
    {
        var data = new SortedDictionary<int, MalibSetItem>();
        var input = new ActionBlock<SetItemNode>(async node =>
        {
            var result = parser.Parse(node, new SetItemParseContext
            {
                ItemOptionData = await itemOptions.ConfigureAwait(false)
            });
            data.Add(result.Id, result);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Input = input;
        Completion = input.Completion;
        Result = CompleteAsync();

        async Task<SortedDictionary<int, MalibSetItem>> CompleteAsync()
        {
            await Completion.ConfigureAwait(false);
            return data;
        }
    }

    public PipelineId Id => PipelineIds.SetItemData;
    public ITargetBlock<SetItemNode> Input { get; }
    public Task<SortedDictionary<int, MalibSetItem>> Result { get; }
    public Task Completion { get; }
}