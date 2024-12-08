namespace WzJson.Domain;

public class SoulSkillInfo
{
    public int SoulSkill { get; set; }
    public int? SoulSkillH { get; set; }
    public Dictionary<string, int> SoulList { get; } = new();
}