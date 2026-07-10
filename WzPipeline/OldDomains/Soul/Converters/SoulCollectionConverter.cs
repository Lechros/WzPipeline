using WzPipeline.Core.Stereotype;
using WzPipeline.OldDomains.Soul.Models;
using WzPipeline.OldDomains.Soul.Nodes;

namespace WzPipeline.OldDomains.Soul.Converters;

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