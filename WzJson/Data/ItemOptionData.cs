using WzJson.Common;
using WzJson.Model;

namespace WzJson.Data;

public class ItemOptionData : DefaultKeyValueData<ItemOption>
{
    public ItemOption.LevelInfo GetItemOptionLevelInfo(int code, int level)
    {
        return this[code.ToString()].Level[level];
    }

    public GearOption GetGearOption(int code, int level)
    {
        return GetItemOptionLevelInfo(code, level).Option;
    }

    public GearOption GetGearOptionByReqLevel(int code, int reqLevel)
    {
        return GetGearOption(code, GetOptionLevel(reqLevel));
    }

    private int GetOptionLevel(int reqLevel)
    {
        if (reqLevel <= 0) return 0;
        if (reqLevel >= 250) return 25;
        return (reqLevel + 9) / 10;
    }
}