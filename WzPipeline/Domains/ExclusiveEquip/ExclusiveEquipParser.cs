using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Domains.ExclusiveEquip;

public class ExclusiveEquipParser
{
    public MalibExclusiveEquip Parse(ExclusiveEquipNode node, ExclusiveEquipParseContext context)
    {
        return new MalibExclusiveEquip
        {
            ItemIds = node.ItemIds,
            Names = ConvertGearNames(node, context.GearStringData)
        };
    }

    private string[] ConvertGearNames(ExclusiveEquipNode node, IReadOnlyDictionary<string, NameDesc> gearStringData)
    {
        if (!string.IsNullOrEmpty(node.Info))
        {
            var group = node.Info;
            var delStr = "는 중복 착용이 불가능합니다.";
            if (group.Contains(delStr))
            {
                group = group.Replace(delStr, "");
            }
            else
            {
                group += "류 아이템";
            }

            return [group];
        }
        else
        {
            var names = node.ItemIds.Select(itemId => GetGearName(itemId, gearStringData)).Distinct().ToArray();
            return names.Length == 1 ? [] : names;
        }
    }

    private string GetGearName(int itemId, IReadOnlyDictionary<string, NameDesc> gearStringData)
    {
        return gearStringData[itemId.ToString()].Name ??
               throw new ApplicationException($"Gear id {itemId} doesn't exist.");
    }
}