namespace WzPipeline.OldDomains.Gear.Models;

public class RawGear
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Desc { get; init; }
    public GearPotential[]? Potentials { get; init; }
    public required Dictionary<GearPropType, int> Props { get; init; }
    public int[]? ReqSpecJobs { get; init; }
}