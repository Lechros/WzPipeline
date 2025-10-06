using WzJson.Core.Stereotype;

namespace WzJson.Domains.Soul.Nodes;

public interface ISoulCollectionNode : INode
{
    public int SoulSkill { get; }
    public int? SoulSkillH { get; }
    public int[][] SoulList { get; }
}