using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.ItemOption;

public sealed class ItemOptionDataBuilder(WzTree tree, ItemOptionParser parser)
{
    public async Task<ItemOptionData> BuildAsync()
    {
        var data = new ItemOptionData();

        var source = tree.MatchNodes(ItemOptionSource.Pattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, ItemOptionNode>(node => new ItemOptionNode(node));
        var parse = new TransformBlock<ItemOptionNode, ItemOptionEntry>(parser.Parse);
        var collector = DataflowCollectors.DictionaryCollector(data, e => e.Code);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}