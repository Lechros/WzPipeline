using WzPipeline.Core.Stereotype;

namespace WzPipeline.OldDomains.Soul.Nodes;

public interface ISoulCollectionNode : INode
{
    public int SoulSkill { get; }
    public int? SoulSkillH { get; }
    public int[][] SoulList { get; }
}