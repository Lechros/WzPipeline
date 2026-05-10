using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Gear.Models;
using WzPipeline.Domains.SubWeaponTransfer;

namespace WzPipeline.Domains.Gear.Processors;

public class GearAstraSubWeaponProcessor(IAstraSubWeaponData astraSubWeaponData)
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

            var fullJobs = GetFullJobs(gear.Id);
            if (fullJobs != null)
            {
                if (gear.Req.Job.FullJobs != null && gear.Req.Job.FullJobs.Length > 0)
                {
                    if (!gear.Req.Job.FullJobs.SequenceEqual(fullJobs))
                    {
                        throw new InvalidDataException(
                            $"Gear {gear.Id} already contains different fullJobs {string.Join(",", gear.Req.Job.FullJobs)} != {string.Join(",", fullJobs)}");
                    }
                }
                else
                {
                    gear.Req.Job.FullJobs = fullJobs;
                }
            }

            yield return gear;
        }
    }

    private int[]? GetFullJobs(int gearId)
    {
        if (astraSubWeaponData.TryGetValue(gearId, out var entry))
        {
            // 썬콜, 불독, 비숍 순서가 되도록 보정
            if (entry.Jobs.SequenceEqual([212, 222, 232]))
            {
                return [222, 212, 232];
            }

            return entry.Jobs.ToArray();
        }

        return null;
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