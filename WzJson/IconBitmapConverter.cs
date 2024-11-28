using System.Drawing;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson;

public class IconBitmapConverter : AbstractNodeConverter<Bitmap>
{
    private readonly string dataName;
    private readonly string iconNodePath;
    private readonly GlobalFindNodeFunction findNodeFunction;

    public IconBitmapConverter(string dataName, string iconNodePath, GlobalFindNodeFunction findNodeFunction)
    {
        this.dataName = dataName;
        this.iconNodePath = iconNodePath;
        this.findNodeFunction = findNodeFunction;
    }

    public override IData NewData()
    {
        return new BitmapData(dataName);
    }

    public override string GetNodeName(Wz_Node node)
    {
        var code = WzUtility.GetNodeCode(node);
        return $"{code}.png";
    }

    public override Bitmap? ConvertNode(Wz_Node node, string _)
    {
        var iconNode = node.FindNodeByPath(iconNodePath);
        if (iconNode == null) return null;
        var resolvedIconNode = WzUtility.ResolveLinkedNode(iconNode, findNodeFunction);
        return resolvedIconNode.GetValue<Wz_Png>().ExtractPng();
    }
}