using System.Drawing;
using WzComparerR2;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzPipeline.Wz;

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

    public Bitmap Image
    {
        get
        {
            var wzPng = linkedSourceNode.GetValue<Wz_Png>();
            lock (wzPng.WzFile.ReadLock)
            {
                return wzPng.ExtractPng();
            }
        }
    }

    public Point? Origin
    {
        get
        {
            var originNode = node.Nodes["origin"];
            if (originNode == null)
            {
                return null;
            }

            var vector = originNode.GetValue<Wz_Vector>();
            return new Point(vector.X, vector.Y);
        }
    }

    public static IconNode Create(string id, Wz_Node node, GlobalFindNodeFunction findNode)
    {
        ArgumentNullException.ThrowIfNull(node);
        node = node.HandleFullUol(findNode);
        var linkedSourceNode = node.GetLinkedSourceNodeThreadSafe(findNode)
                               ?? throw new InvalidOperationException(
                                   $"Linked icon source was not found: {node.FullPath}");
        return new IconNode(id, node, linkedSourceNode);
    }
}