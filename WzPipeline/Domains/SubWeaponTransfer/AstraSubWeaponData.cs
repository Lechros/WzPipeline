namespace WzPipeline.Domains.SubWeaponTransfer;

public interface IAstraSubWeaponData : IReadOnlyDictionary<int, AstraSubWeaponEntry>
{
    public int GetAstraIndex(int itemId);
}

public class AstraSubWeaponData : Dictionary<int, AstraSubWeaponEntry>, IAstraSubWeaponData
{
    public int GetAstraIndex(int itemId)
    {
        if (itemId / 10000 == 172)
            return itemId % 10;

        if (TryGetValue(itemId, out var value))
            return value.Index;

        return -1;
    }
}