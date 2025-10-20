using WzComparerR2.WzLib;
using WzPipeline.Core.Stereotype;

namespace WzPipeline.Shared.Node;

public class WzNode(Wz_Node node) : INode
{
    public static WzNode Create(Wz_Node node)
    {
        return new WzNode(node);
    }

    public string Id => node.Text;
    public Wz_Node Node => node;
}