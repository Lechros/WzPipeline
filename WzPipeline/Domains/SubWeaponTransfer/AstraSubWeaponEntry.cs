namespace WzPipeline.Domains.SubWeaponTransfer;

public class AstraSubWeaponEntry
{
    public required int Id { get; init; }
    public required List<int> Jobs { get; init; }
    public required int Index { get; init; }
}