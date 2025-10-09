using WzPipeline.Domains.Gear.Models;

namespace WzPipeline.Domains.Soul.Models;

public class SkillOption
{
    public required int SkillId { get; init; }
    public required int IncTableId { get; init; }
    public required GearOption[] Options { get; init; }
}