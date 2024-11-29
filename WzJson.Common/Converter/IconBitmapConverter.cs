using System.Drawing;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common.Data;

namespace WzJson.Common.Converter;

public class IconBitmapConverter(string dataName, string iconNodePath, GlobalFindNodeFunction findNodeFunction)
    : AbstractNodeConverter<Bitmap>
{
    public override IData NewData() => new BitmapData(dataName);

    public override string GetNodeKey(Wz_Node node) => $"{WzUtility.GetNodeCode(node)}.png";

    public override Bitmap? ConvertNode(Wz_Node node, string _)
    {
        var iconNode = node.FindNodeByPath(iconNodePath);
        if (iconNode == null) return null;
        var resolvedIconNode = WzUtility.ResolveLinkedNode(iconNode, findNodeFunction);
        return resolvedIconNode.GetValue<Wz_Png>().ExtractPng();
    }
}