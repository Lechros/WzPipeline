using WzJson.V2.Core.Stereotype;
using WzJson.V2.Domains.Gear.Models;
using WzJson.V2.Domains.ItemOption;
using WzJson.V2.Domains.Soul.Models;
using WzJson.V2.Domains.Soul.Nodes;

namespace WzJson.V2.Domains.Soul.Converters;

public class SkillOptionConverter(IReadOnlyDictionary<int, ItemOptionEntry> itemOptionData)
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