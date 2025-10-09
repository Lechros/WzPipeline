using WzPipeline.Core.Stereotype;
using WzPipeline.Domains.Soul.Models;
using WzPipeline.Domains.Soul.Nodes;

namespace WzPipeline.Domains.Soul.Converters;

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