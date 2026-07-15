using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Domains.Soul;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.Soul;

public class SoulInfoDataBuilder(WzTree tree, SoulCollectionParser parser)
{
    public async Task<Dictionary<int, SoulInfo>> BuildAsync()
    {
        var data = new Dictionary<int, SoulInfo>();

        var source = tree.MatchNodes(SoulSource.SoulCollectionPattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, SoulCollectionNode>(node => new SoulCollectionNode(node));
        var parse = new TransformManyBlock<SoulCollectionNode, SoulInfo>(parser.Parse);
        var collector = DataflowCollectors.DictionaryCollector(data, e => e.SoulId);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}