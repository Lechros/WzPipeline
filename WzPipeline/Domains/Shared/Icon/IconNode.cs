using System.Drawing;
using WzComparerR2;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Shared.Icon;

public class IconNode
{
    private readonly Wz_Node node;
    private readonly Wz_Node linkedSourceNode;

    private IconNode(string id, Wz_Node node, Wz_Node linkedSourceNode)
    {
        Id = id;
        this.node = node;
        this.linkedSourceNode = linkedSourceNode;
    }

    public string Id { get; }
    public Bitmap Image => linkedSourceNode.GetValue<Wz_Png>().ExtractPng();
    public Point Origin
    {
        get
        {
            var originNode = node.Nodes["origin"];
            var vector = originNode.GetValue<Wz_Vector>();
            return new Point(vector.X, vector.Y);
        }
    }

    public static IconNode Create(string id, Wz_Node node, GlobalFindNodeFunction findNode)
    {
        node = node.HandleFullUol(findNode);
        return new IconNode(id, node, node.GetLinkedSourceNode(findNode));
    }
}