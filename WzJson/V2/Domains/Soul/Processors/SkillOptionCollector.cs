using WzJson.V2.Core.Stereotype;
using WzJson.V2.Domains.Soul.Models;

namespace WzJson.V2.Domains.Soul.Processors;

public class SkillOptionCollector : AbstractProcessor<SkillOption, Dictionary<int, List<SkillOption>>>
{
    public override IEnumerable<Dictionary<int, List<SkillOption>>> Process(IEnumerable<SkillOption> skillOptions)
    {
        yield return skillOptions
            .GroupBy(so => so.SkillId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}