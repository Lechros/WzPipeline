using WzJson.Core.Stereotype;
using WzJson.Domains.Soul.Models;

namespace WzJson.Domains.Soul.Processors;

public class SkillOptionCollector : AbstractProcessor<SkillOption, Dictionary<int, List<SkillOption>>>
{
    public override IEnumerable<Dictionary<int, List<SkillOption>>> Process(IEnumerable<SkillOption> skillOptions)
    {
        yield return skillOptions
            .GroupBy(so => so.SkillId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}