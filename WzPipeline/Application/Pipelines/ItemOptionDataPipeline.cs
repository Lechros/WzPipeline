using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Domains.Shared.ItemOption;

namespace WzPipeline.Application.Pipelines;

public sealed class ItemOptionDataPipeline : IPipeline
{
    public PipelineId Id => PipelineIds.ItemOptionData;
    public ITargetBlock<ItemOptionNode> Input { get; }
    public Task<ItemOptionData> Result { get; }
    public Task Completion { get; }

    public ItemOptionDataPipeline(ItemOptionParser parser, CancellationToken cancellationToken = default)
    {
        var parserBlock = new TransformBlock<ItemOptionNode, ItemOptionEntry>(parser.Parse,
            new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });

        var data = new ItemOptionData();
        var collectorBlock = DataflowCollectors.DictionaryCollector(data, static e => e.Code, cancellationToken);

        parserBlock.LinkTo(collectorBlock, new DataflowLinkOptions { PropagateCompletion = true });

        Input = parserBlock;
        Completion = collectorBlock.Completion;
        Result = GetDataAsync();

        return;

        async Task<ItemOptionData> GetDataAsync()
        {
            await collectorBlock.Completion.ConfigureAwait(false);
            return data;
        }
    }
}