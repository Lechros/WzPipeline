using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Dataflow;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.DataBuilders;

public class SkillNameDataBuilder(WzTree tree)
{
    public async Task<Dictionary<string, string>> BuildAsync()
    {
        var data = new Dictionary<string, string>();

        var source = tree.MatchNodes(StringSource.SkillNamePattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, StringNode>(node => new StringNode(node));
        var parse = new TransformBlock<StringNode, KeyValuePair<string, string>>(node =>
            KeyValuePair.Create(node.Key, node.Name!));
        var collector = DataflowCollectors.DictionaryCollector(data);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}