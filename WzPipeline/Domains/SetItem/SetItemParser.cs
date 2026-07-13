using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Shared.ItemOption;

namespace WzPipeline.Domains.SetItem;

public class SetItemParser
{
    public MalibSetItem Parse(SetItemNode node, SetItemParseContext context)
    {
        return new MalibSetItem
        {
            Id = int.Parse(node.Id),
            Name = node.Name,
            ItemIds = node.ItemIds.ToArray(),
            Effects = ConvertEffects(node.Effects, context.ItemOptionData),
            JokerPossible = node.JokerPossible,
            ZeroWeaponJokerPossible = node.ZeroWeaponJokerPossible
        };
    }

    private static SortedDictionary<int, GearOption> ConvertEffects(IEnumerable<SetItemNode.EffectNode> effectNodes,
        ItemOptionData itemOptionData)
    {
        var result = new SortedDictionary<int, GearOption>();
        foreach (var effectNode in effectNodes)
        {
            var option = new GearOption();
            foreach (var (optionCode, level) in effectNode.Options)
                option.Add(itemOptionData[optionCode].Level[level].Option);

            foreach (var (type, value) in effectNode.Properties)
            {
                if (!Enum.TryParse(type, out GearPropType prop))
                    throw new NotImplementedException("Unknown set item property type: " + type);

                option.Add(prop, value);
            }

            result.Add(effectNode.Index, option);
        }

        return result;
    }
}