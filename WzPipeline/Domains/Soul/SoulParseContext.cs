namespace WzPipeline.Domains.Soul;

public class SoulParseContext
{
    public required IReadOnlyDictionary<string, string> ConsumeNameData { get; init; }
    public required IReadOnlyDictionary<int, SoulInfo> SoulInfoData { get; init; }
    public required IReadOnlyDictionary<int, SkillOption> SkillOptionData { get; init; }
    public required IReadOnlySet<int> SoulAmplifyData { get; init; }
}