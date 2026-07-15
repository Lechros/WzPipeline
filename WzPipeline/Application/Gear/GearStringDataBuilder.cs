using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.Gear;

public class GearStringDataBuilder(WzTree tree)
{
    public async Task<Dictionary<string, NameDesc>> BuildAsync()
    {
        var data = new Dictionary<string, NameDesc>();

        var source = tree.MatchNodes(StringSource.GearNamePattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, StringNode>(node => new StringNode(node));
        var parse = new TransformBlock<StringNode, KeyValuePair<string, NameDesc>>(node =>
            KeyValuePair.Create(node.Key, new NameDesc(node.Name, node.Desc)));
        var collector = DataflowCollectors.DictionaryCollector(data);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}