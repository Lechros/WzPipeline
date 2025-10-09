namespace WzPipeline.Domains.Soul.Models;

public interface ISkillOptionData : IReadOnlyDictionary<int, List<SkillOption>>
{
}

public class SkillOptionData : Dictionary<int, List<SkillOption>>, ISkillOptionData
{
}