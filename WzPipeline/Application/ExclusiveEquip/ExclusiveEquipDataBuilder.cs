using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Gear;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Domains.ExclusiveEquip;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.ExclusiveEquip;

public class ExclusiveEquipDataBuilder(
    WzTree tree,
    ExclusiveEquipParser parser,
    GearStringDataProvider gearStringDataProvider)
{
    public async Task<SortedDictionary<int, MalibExclusiveEquip>> BuildAsync()
    {
        var gearStringData = await gearStringDataProvider.GetAsync();
        var context = new ExclusiveEquipParseContext
        {
            GearStringData = gearStringData
        };

        var data = new SortedDictionary<int, MalibExclusiveEquip>();

        var source = tree.MatchNodes(ExclusiveEquipSource.Pattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, ExclusiveEquipNode>(node => new ExclusiveEquipNode(node));
        var parse = new TransformBlock<ExclusiveEquipNode, KeyValuePair<int, MalibExclusiveEquip>>(node =>
            KeyValuePair.Create(int.Parse(node.Id), parser.Parse(node, context)));
        var collector = DataflowCollectors.DictionaryCollector(data);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}