using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Dataflow;
using WzPipeline.Application.DataProviders;
using WzPipeline.Domains.Gear;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.DataBuilders;

public class GearDataBuilder(
    WzTree tree,
    GearParser parser,
    GearStringDataProvider gearStringDataProvider,
    ItemOptionDataProvider itemOptionDataProvider,
    SkillNameDataProvider skillNameDataProvider,
    AstraSubWeaponDataProvider astraSubWeaponDataProvider)
{
    public async Task<SortedDictionary<int, MalibGear>> BuildAsync()
    {
        var gearStringDataTask = gearStringDataProvider.GetAsync();
        var itemOptionDataTask = itemOptionDataProvider.GetAsync();
        var skillNameDataTask = skillNameDataProvider.GetAsync();
        var astraSubWeaponDataTask = astraSubWeaponDataProvider.GetAsync();

        await Task.WhenAll(gearStringDataTask, itemOptionDataTask, skillNameDataTask, astraSubWeaponDataTask);
        var context = new GearParseContext
        {
            GearStringData = await gearStringDataTask,
            ItemOptionData = await itemOptionDataTask,
            SkillNameData = await skillNameDataTask,
            AstraSubWeaponData = await astraSubWeaponDataTask
        };

        var data = new SortedDictionary<int, MalibGear>();

        var source = tree.MatchNodes(GearSource.Pattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, GearNode>(node => new GearNode(node));
        var parse = new TransformManyBlock<GearNode, MalibGear>(node => parser.Parse(node, context));
        var collector = DataflowCollectors.DictionaryCollector(data, e => e.Id);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}