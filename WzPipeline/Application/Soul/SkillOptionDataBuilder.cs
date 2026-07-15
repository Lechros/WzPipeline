using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Application.ItemOption;
using WzPipeline.Domains.Soul;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.Soul;

public class SkillOptionDataBuilder(
    WzTree tree,
    SkillOptionParser parser,
    ItemOptionDataProvider itemOptionDataProvider)
{
    public async Task<Dictionary<int, IList<SkillOption>>> BuildAsync()
    {
        var itemOptionData = await itemOptionDataProvider.GetAsync();
        var context = new SkillOptionParseContext
        {
            ItemOptionData = itemOptionData
        };

        var data = new Dictionary<int, IList<SkillOption>>();

        var source = tree.MatchNodes(SoulSource.SkillOptionPattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, SkillOptionNode>(node => new SkillOptionNode(node));
        var parse = new TransformManyBlock<SkillOptionNode, SkillOption>(node => parser.Parse(node, context));
        var collector = new ActionBlock<SkillOption>(item =>
        {
            if (!data.TryGetValue(item.SkillId, out var list))
            {
                list = [];
                data.Add(item.SkillId, list);
            }

            list.Add(item);
        }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }
}