namespace WzJson.Domains.Soul.Models;

public class SoulInfo
{
    public required int SoulId { get; init; }
    public required int SkillId { get; init; }
    public required int Index { get; init; }

    public bool IsMagnificent => Index == 8;
}