namespace WzJson.Domains.String;

public interface ISkillNameData : IReadOnlyDictionary<string, string>
{
}

public class SkillNameData : Dictionary<string, string>, ISkillNameData
{
}