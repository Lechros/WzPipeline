using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Gear.Models;
using WzPipeline.Domains.ItemOption;
using WzPipeline.Domains.Soul.Models;
using WzPipeline.Domains.Soul.Nodes;

namespace WzPipeline.Domains.Soul.Converters;

public class SkillOptionConverter(IItemOptionData itemOptionData)
    : AbstractConverter<ISkillOptionNode, SkillOption>
{
    public override SkillOption? Convert(ISkillOptionNode node)
    {
        if (node.IncTableId == 0)
        {
            return null;
        }

        return new SkillOption
        {
            SkillId = node.SkillId,
            IncTableId = node.IncTableId,
            Options = node.TempOption.Select(ConvertToGearOption).ToArray()
        };
    }

    private GearOption ConvertToGearOption(ISkillOptionNode.ITempOption tempOption)
    {
        var itemOption = itemOptionData[tempOption.Id];
        var level = ItemOptionUtils.GetItemOptionLevel(75);
        var levelOption = itemOption.Level[level];
        return levelOption.Option;
    }
}