using WzJson.V2.Core.Stereotype;
using WzJson.V2.Domains.Gear.Models;
using WzJson.V2.Domains.Gear.Nodes;
using WzJson.V2.Domains.String;

namespace WzJson.V2.Domains.Gear.Converters;

public class RawGearConverter(IReadOnlyDictionary<string, NameDesc> gearStringData)
    : AbstractConverter<IGearNode, RawGear>
{
    public override RawGear? Convert(IGearNode node)
    {
        gearStringData.TryGetValue(node.Id, out var gearNameDesc);
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
        };

        foreach (var (type, value) in node.Properties)
        {
            if (!Enum.TryParse(type, out GearPropType prop))
            {
                throw new NotImplementedException("Unknown gear property type: " + type);
            }

            gear.Props.Add(prop, value);
        }

        // TODO: Potentials
        
        return gear;
    }
}