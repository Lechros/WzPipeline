using System.Drawing;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson.Gear;

public class IconBitmapConverter : INodeConverter<BitmapData>
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

    public BitmapData Convert(IEnumerable<Wz_Node> nodes)
    {
        var data = new BitmapData(dataName);
        foreach (var node in nodes)
        {
            var code = WzUtility.GetNodeCode(node);
            var icon = ConvertNode(node);
            if (icon != null)
                data.Items.Add($"{code}.png", icon);
        }

        return data;
    }

    public Bitmap? ConvertNode(Wz_Node node)
    {
        var iconNode = node.FindNodeByPath(iconNodePath);
        if (iconNode == null) return null;
        var resolvedIconNode = WzUtility.ResolveLinkedNode(iconNode, findNodeFunction);
        return resolvedIconNode.GetValue<Wz_Png>().ExtractPng();
    }
}