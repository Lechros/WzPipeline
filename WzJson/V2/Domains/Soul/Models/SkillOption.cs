using WzJson.V2.Domains.Gear.Models;

namespace WzJson.V2.Domains.Soul.Models;

public class SkillOption
{
    public required int SkillId { get; init; }
    public required int IncTableId { get; init; }
    public required GearOption[] Options { get; init; }
}