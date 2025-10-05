namespace WzJson.V2.Domains.Soul.Models;

public class SoulCollection
{
    public required int SoulSkill { get; init; }
    public required int? SoulSkillH { get; init; }
    public required int[][] SoulList { get; init; }
}