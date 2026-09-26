using WzPipeline.Domains.Shared;

namespace WzPipeline.Domains.Soul;

public class SkillOption
{
    public int SkillOptionId { get; init; }
    public int IncTableId { get; init; }
    public required GearOption[] Options { get; init; }
}