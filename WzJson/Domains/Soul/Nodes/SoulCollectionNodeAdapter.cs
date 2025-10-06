using WzComparerR2.WzLib;

namespace WzJson.Domains.Soul.Nodes;

public class SoulCollectionNodeAdapter(Wz_Node node) : ISoulCollectionNode
{
    public static SoulCollectionNodeAdapter Create(Wz_Node node)
    {
        return new SoulCollectionNodeAdapter(node);
    }

    public string Id => node.Text;
    public int SoulSkill => node.Nodes["soulSkill"].GetValue<int>();
    public int? SoulSkillH => node.Nodes["soulSkillH"]?.GetValue<int>();
    public int[][] SoulList => node.Nodes["soulList"].Nodes.Select(SubNodeToArray).ToArray();

    private static int[] SubNodeToArray(Wz_Node node)
    {
        return node.Nodes.Select(n => n.GetValue<int>()).ToArray();
    }
}