using System.Drawing;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson.Domains.Icon;

public class IconUtility
{
    public static Bitmap? GetIconImage(Wz_Node node, string path, GlobalFindNodeFunction findNode)
    {
        var iconNode = node.FindNodeByPath(false, path.Split("/"));
        if (iconNode == null) return null;
        iconNode = iconNode.HandleFullUol(findNode).GetLinkedSourceNode(findNode);
        return iconNode.GetValue<Wz_Png>().ExtractPng();
    }

    public static Point? GetIconOrigin(Wz_Node node, string path, GlobalFindNodeFunction findNode)
    {
        var iconNode = node.FindNodeByPath(false, path.Split("/"));
        if (iconNode == null) return null;
        iconNode = iconNode.HandleFullUol(findNode);
        var originNode = iconNode.Nodes["origin"];
        if (originNode == null) return null;
        var vector = originNode.GetValue<Wz_Vector>();
        return new Point(vector.X, vector.Y);
    }
}