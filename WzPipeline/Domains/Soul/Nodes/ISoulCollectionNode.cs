using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.Soul.Nodes;

public interface ISoulCollectionNode : INode
{
    public int SoulSkill { get; }
    public int? SoulSkillH { get; }
    public int[][] SoulList { get; }
}