using WzPipeline.Core.Stereotype;
using WzPipeline.OldDomains.Gear.Models;
using WzPipeline.OldDomains.Gear.Nodes;
using WzPipeline.OldDomains.ItemOption;

namespace WzPipeline.OldDomains.Gear.Converters;

public class RawGearConverter(
    IGearNameDescData gearNameDescData,
    IItemOptionData itemOptionData)
    : AbstractConverter<IGearNode, RawGear>
{
    public override RawGear? Convert(IGearNode node)
    {
        gearNameDescData.TryGetValue(node.Id, out var gearNameDesc);
        if (gearNameDesc?.Name == null)
        {
            return null;
        }

        if (node.IsCash)
        {
            return null;
        }

        var gear = new RawGear
        {
            Id = int.Parse(node.Id),
            Name = gearNameDesc.Name,
            Desc = gearNameDesc.Desc,
            Props = ConvertProps(node.Properties),
            Potentials = ConvertGearPotentials(node.Options),
            ReqSpecJobs = ConvertReqSpecJobs(node.ReqSpecJobs)
        };

        return gear;
    }

    private Dictionary<GearPropType, int> ConvertProps(IEnumerable<(string, int)> props)
    {
        var dict = new Dictionary<GearPropType, int>();
        foreach (var (type, value) in props)
        {
            if (!Enum.TryParse(type, out GearPropType prop))
            {
                throw new NotImplementedException("Unknown gear property type: " + type);
            }

            dict.Add(prop, value);
        }

        return dict;
    }

    private GearPotential[]? ConvertGearPotentials((int, int)[]? options)
    {
        if (options == null)
        {
            return null;
        }

        var potentials = new List<GearPotential>(options.Length);
        foreach (var (optionCode, level) in options)
        {
            var itemOption = itemOptionData[optionCode];
            var levelOption = itemOption.Level[level];
            var grade = levelOption.FixedGrade != null
                ? GetPotentialGradeFromFixedGrade(levelOption.FixedGrade.Value)
                : optionCode / 10000;
            var potential = new GearPotential
            {
                Id = optionCode,
                Grade = grade,
                Summary = levelOption.String,
                Option = levelOption.Option
            };
            potentials.Add(potential);
        }

        return potentials.ToArray();
    }

    private int[] ConvertReqSpecJobs(IEnumerable<int> reqSpecJobs)
    {
        return reqSpecJobs.ToArray();
    }

    private int GetPotentialGradeFromFixedGrade(int fixedGrade)
    {
        return fixedGrade switch
        {
            2 => 1,
            3 => 2,
            5 => 3,
            7 => 4,
            _ => fixedGrade - 1
        };
    }
}