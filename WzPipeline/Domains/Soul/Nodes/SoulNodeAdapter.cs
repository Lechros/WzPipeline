using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Soul.Nodes;

public class SoulNodeAdapter(Wz_Node node) : ISoulNode
{
    public static SoulNodeAdapter Create(Wz_Node node)
    {
        return new SoulNodeAdapter(node);
    }

    public string Id => int.Parse(node.Text.Split('.')[0]).ToString();
}