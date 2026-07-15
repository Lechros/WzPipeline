using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Soul;

public class SoulCollectionNode(Wz_Node node)
{
    public string Id => node.Text;
    public int SoulSkill => node.Nodes["soulSkill"].GetValue<int>();
    public int? SoulSkillH => node.Nodes["soulSkillH"]?.GetValue<int>();
    public int[][] SoulList => node.Nodes["soulList"].Nodes.Select(SoulListNodeToArray).ToArray();

    private static int[] SoulListNodeToArray(Wz_Node node)
    {
        return node.Nodes.Select(n => n.GetValue<int>()).ToArray();
    }
}