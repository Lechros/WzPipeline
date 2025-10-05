using WzJson.V2.Core.Stereotype;
using WzJson.V2.Domains.Soul.Models;
using WzJson.V2.Domains.Soul.Nodes;

namespace WzJson.V2.Domains.Soul.Converters;

public class SoulCollectionConverter : AbstractConverter<ISoulCollectionNode, SoulCollection>
{
    public override SoulCollection? Convert(ISoulCollectionNode node)
    {
        return new SoulCollection
        {
            SoulSkill = node.SoulSkill,
            SoulSkillH = node.SoulSkillH,
            SoulList = node.SoulList
        };
    }
}