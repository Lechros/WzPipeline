namespace WzPipeline.Domains.Soul;

public class SoulParseContext
{
    public required IReadOnlyDictionary<string, string> ConsumeNameData { get; init; }
    public required IReadOnlyDictionary<string, string> SkillNameData { get; init; }
    public required IReadOnlyDictionary<int, SoulInfo> SoulInfoData { get; init; }
    public required IReadOnlyDictionary<int, IList<SkillOption>> SkillOptionData { get; init; }
}