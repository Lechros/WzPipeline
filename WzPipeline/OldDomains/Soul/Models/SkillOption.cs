using WzPipeline.OldDomains.Gear.Models;

namespace WzPipeline.OldDomains.Soul.Models;

public class SkillOption
{
    public required int SkillId { get; init; }
    public required int IncTableId { get; init; }
    public required GearOption[] Options { get; init; }
}