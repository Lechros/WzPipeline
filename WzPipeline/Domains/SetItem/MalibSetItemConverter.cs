using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.Gear.Models;
using WzPipeline.Domains.ItemOption;

namespace WzPipeline.Domains.SetItem;

public class MalibSetItemConverter(IItemOptionData itemOptionData)
    : AbstractConverter<ISetItemNode, MalibSetItem>
{
    public override MalibSetItem? Convert(ISetItemNode node)
    {
        return new MalibSetItem
        {
            Id = int.Parse(node.Id),
            Name = node.Name,
            ItemIds = node.ItemIds.ToArray(),
            Effects = ConvertEffects(node.Effects),
            JokerPossible = node.JokerPossible,
            ZeroWeaponJokerPossible = node.ZeroWeaponJokerPossible
        };
    }

    private SortedDictionary<int, GearOption> ConvertEffects(IEnumerable<ISetItemNode.IEffectNode> effectNodes)
    {
        var result = new SortedDictionary<int, GearOption>();
        foreach (var effectNode in effectNodes)
        {
            var option = new GearOption();
            foreach (var (optionCode, level) in effectNode.Options)
            {
                option.Add(itemOptionData[optionCode].Level[level].Option);
            }

            foreach (var (type, value) in effectNode.Properties)
            {
                if (!Enum.TryParse(type, out GearPropType prop))
                {
                    throw new NotImplementedException("Unknown set item property type: " + type);
                }

                option.Add(prop, value);
            }

            result.Add(effectNode.Index, option);
        }

        return result;
    }
}