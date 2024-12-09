namespace WzJson.Domain;

public class SoulSkillNode
{
    public int SoulSkill { get; set; }
    public int? SoulSkillH { get; set; }
    public List<int> SoulList { get; } = new();
}