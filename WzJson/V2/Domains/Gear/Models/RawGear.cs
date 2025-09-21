namespace WzJson.V2.Domains.Gear.Models;

public class RawGear
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Desc { get; init; }
    public GearPotential[]? Potentials { get; set; }
    public Dictionary<GearPropType, int> Props { get; } = new();
}