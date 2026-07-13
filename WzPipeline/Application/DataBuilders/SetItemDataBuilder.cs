using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Dataflow;
using WzPipeline.Application.DataProviders;
using WzPipeline.Domains.SetItem;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.DataBuilders;

public class SetItemDataBuilder(WzTree tree, SetItemParser parser, ItemOptionDataProvider itemOptionDataProvider)
{
    public async Task<SortedDictionary<int, MalibSetItem>> BuildAsync()
    {
        var itemOptionData = await itemOptionDataProvider.GetAsync();
        var context = new SetItemParseContext { ItemOptionData = itemOptionData };

        var data = new SortedDictionary<int, MalibSetItem>();

        var source = tree.MatchNodes(SetItemSource.Pattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, SetItemNode>(node => new SetItemNode(node));
        var parse = new TransformBlock<SetItemNode, MalibSetItem>(node => parser.Parse(node, context));
        var collector = DataflowCollectors.DictionaryCollector(data, e => e.Id);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}