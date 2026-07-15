using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.Dataflow;
using WzPipeline.Application.Item;
using WzPipeline.Application.Skill;
using WzPipeline.Domains.Soul;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.Soul;

public class SoulDataBuilder(
    WzTree tree,
    SoulParser parser,
    ConsumeNameDataProvider consumeNameDataProvider,
    SkillNameDataProvider skillNameDataProvider,
    SoulInfoDataProvider soulInfoDataProvider,
    SkillOptionDataProvider skillOptionDataProvider)
{
    public async Task<SortedDictionary<int, MalibSoul>> BuildAsync()
    {
        var consumeNameDataTask = consumeNameDataProvider.GetAsync();
        var skillNameDataTask = skillNameDataProvider.GetAsync();
        var soulInfoDataTask = soulInfoDataProvider.GetAsync();
        var skillOptionDataTask = skillOptionDataProvider.GetAsync();

        await Task.WhenAll(consumeNameDataTask, skillNameDataTask, soulInfoDataTask, skillOptionDataTask);
        var context = new SoulParseContext
        {
            ConsumeNameData = await consumeNameDataTask,
            SkillNameData = await skillNameDataTask,
            SoulInfoData = await soulInfoDataTask,
            SkillOptionData = await skillOptionDataTask
        };

        var data = new SortedDictionary<int, MalibSoul>();

        var source = tree.MatchNodes(SoulSource.Pattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, SoulNode>(node => new SoulNode(node));
        var parse = new TransformManyBlock<SoulNode, MalibSoul>(node => parser.Parse(node, context));
        var collector = DataflowCollectors.DictionaryCollector(data, e => e.Id);

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}