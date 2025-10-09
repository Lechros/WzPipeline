using WzJson.Core.Stereotype;
using WzJson.Domains.Gear.Models;
using WzJson.Domains.Gear.Nodes;
using WzJson.Domains.ItemOption;
using WzJson.Domains.String;

namespace WzJson.Domains.Gear.Converters;

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
            Potentials = ConvertGearPotentials(node.Options)
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
            var potential = new GearPotential
            {
                Id = optionCode,
                Grade = optionCode / 10000,
                Summary = levelOption.String,
                Option = levelOption.Option
            };
            potentials.Add(potential);
        }

        return potentials.ToArray();
    }
}