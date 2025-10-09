using WzJson.Domains.Gear.Models;

namespace WzJson.Domains.ItemOption;

public interface IItemOptionData : IReadOnlyDictionary<int, ItemOptionEntry>
{
    public GearOption GetGearOption(int optionCode, int level);
}

public class ItemOptionData : Dictionary<int, ItemOptionEntry>, IItemOptionData
{
    public GearOption GetGearOption(int optionCode, int level)
    {
        return this[optionCode].Level[level].Option;
    }
}