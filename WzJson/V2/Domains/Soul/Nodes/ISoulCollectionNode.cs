using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Domains.Soul.Nodes;

public interface ISoulCollectionNode : INode
{
    public int SoulSkill { get; }
    public int? SoulSkillH { get; }
    public int[][] SoulList { get; }
}