using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Gear.Models;
using WzPipeline.Domains.SubWeaponTransfer;

namespace WzPipeline.Domains.Gear.Processors;

public class GearAstraSubWeaponMaxStarProcessor(IAstraSubWeaponData astraSubWeaponData)
    : AbstractProcessor<MalibGear, MalibGear>
{
    public override IEnumerable<MalibGear> Process(IEnumerable<MalibGear> models)
    {
        foreach (var gear in models)
        {
            var maxStar = GetFixedMaxStar(gear.Id);
            if (maxStar != null)
            {
                gear.Attributes.FixedMaxStar = maxStar;
                gear.Attributes.CanStarforce = 1; // GearCapability.Can
            }

            yield return gear;
        }
    }

    private int? GetFixedMaxStar(int gearId)
    {
        switch (astraSubWeaponData.GetAstraIndex(gearId))
        {
            case 0:
                return 15;
            case 1:
                return 20;
            case 2:
                return 30;
            default:
                return null;
        }
    }
}