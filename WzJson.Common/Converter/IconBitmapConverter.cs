using System.Drawing;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common.Data;

namespace WzJson.Common.Converter;

public class IconBitmapConverter(string iconNodePath, GlobalFindNodeFunction findNode)
    : AbstractNodeConverter<Bitmap>
{
    public override string GetNodeKey(Wz_Node node) => $"{WzUtility.GetNodeCode(node)}.png";

    public override Bitmap? Convert(Wz_Node node, string _)
    {
        var iconNode = node.FindNodeByPath(iconNodePath);
        if (iconNode == null) return null;
        var resolvedIconNode = WzUtility.ResolveLinkedNode(iconNode, findNode);
        return resolvedIconNode.GetValue<Wz_Png>().ExtractPng();
    }
}