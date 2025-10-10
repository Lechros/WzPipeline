using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Gear.Models;

namespace WzPipeline.Domains.ExclusiveEquip;

public class ExclusiveEquipConverter(IGearNameDescData gearNameDescData)
    : AbstractConverter<IExclusiveEquipNode, MalibExclusiveEquip>
{
    public override MalibExclusiveEquip? Convert(IExclusiveEquipNode node)
    {
        return new MalibExclusiveEquip
        {
            Id = int.Parse(node.Id),
            ItemIds = node.ItemIds,
            Names = ConvertGearNames(node)
        };
    }

    private string[] ConvertGearNames(IExclusiveEquipNode node)
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
            var names = node.ItemIds.Select(GetGearName).Distinct().ToArray();
            return names.Length == 1 ? [] : names;
        }
    }

    private string GetGearName(int itemId)
    {
        return gearNameDescData[itemId.ToString()].Name ??
               throw new ApplicationException($"Gear id {itemId} doesn't exist.");
    }
}