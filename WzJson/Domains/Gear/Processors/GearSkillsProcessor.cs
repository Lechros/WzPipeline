using WzJson.Core.Stereotype;
using WzJson.Domains.Gear.Models;
using WzJson.Domains.String;

namespace WzJson.Domains.Gear.Processors;

public class GearSkillsProcessor(ISkillNameData skillNameData)
    : AbstractProcessor<MalibGear, MalibGear>
{
    public override IEnumerable<MalibGear> Process(IEnumerable<MalibGear> models)
    {
        foreach (var gear in models)
        {
            yield return AddSkills(gear);
        }
    }

    private MalibGear AddSkills(MalibGear gear)
    {
        if (gear.IsRawAttributes())
        {
            throw new InvalidOperationException($"{nameof(GearSkillsProcessor)} requires processed attributes.");
        }

        var skills = GetSpecialWeaponSkills(gear);
        gear.Attributes.Skills.AddRange(skills.Select(id => skillNameData[id.ToString()]));

        return gear;
    }

    private int[] GetSpecialWeaponSkills(MalibGear gear)
    {
        if (gear.Type.IsWeapon() && gear.Attributes.SetItemId is >= 886 and <= 890)
        {
            return gear.Req.Level switch
            {
                // Genesis
                200 => [80002632, 80002633],
                // Destiny
                250 => [80003873, 80003874],
                _ => []
            };
        }

        return [];
    }
}