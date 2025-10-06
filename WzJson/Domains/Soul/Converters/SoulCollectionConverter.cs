using WzJson.Core.Stereotype;
using WzJson.Domains.Soul.Models;
using WzJson.Domains.Soul.Nodes;

namespace WzJson.Domains.Soul.Converters;

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