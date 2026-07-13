namespace WzPipeline.Domains.AstraSubWeapon;

public class AstraSubWeaponData : Dictionary<int, AstraSubWeaponEntry>
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

public class AstraSubWeaponEntry
{
    public required int Id { get; init; }
    public required List<int> Jobs { get; init; }
    public required int Index { get; init; }
}