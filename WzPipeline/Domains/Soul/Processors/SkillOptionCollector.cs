using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Soul.Models;

namespace WzPipeline.Domains.Soul.Processors;

public class SkillOptionCollector : AbstractProcessor<SkillOption, SkillOptionData>
{
    public override IEnumerable<SkillOptionData> Process(IEnumerable<SkillOption> skillOptions)
    {
        var result = new SkillOptionData();
        foreach (var grouping in skillOptions.GroupBy(so => so.SkillId))
        {
            result[grouping.Key] = grouping.ToList();
        }

        yield return result;
    }
}