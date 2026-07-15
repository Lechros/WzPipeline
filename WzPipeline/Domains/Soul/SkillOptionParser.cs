using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Shared.ItemOption;

namespace WzPipeline.Domains.Soul;

public class SkillOptionParser
{
    public IEnumerable<SkillOption> Parse(SkillOptionNode node, SkillOptionParseContext context)
    {
        if (node.IncTableId == 0)
        {
            yield break;
        }

        yield return new SkillOption
        {
            SkillId = node.SkillId,
            IncTableId = node.IncTableId,
            Options = node.TempOption.Select(o => ConvertToGearOption(o, context.ItemOptionData)).ToArray()
        };
    }

    private static GearOption ConvertToGearOption(SkillOptionNode.TempOptionNode node, ItemOptionData itemOptionData)
    {
        var itemOption = itemOptionData[node.Id];
        var level = ItemOptionUtils.GetItemOptionLevel(75);
        var levelOption = itemOption.Level[level];
        return levelOption.Option;
    }
}