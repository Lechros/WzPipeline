namespace WzJson.Domains.ItemOption;

public static class ItemOptionUtils
{
    public static int GetItemOptionLevel(int reqLevel)
    {
        if (reqLevel <= 0) return 0;
        if (reqLevel >= 250) return 25;
        return (reqLevel + 9) / 10;
    }
}