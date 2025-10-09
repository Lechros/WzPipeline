using System.Drawing;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Icon;

public class DefaultIconNodeAdapter(Wz_Node node, Wz_Node infoNode, GlobalFindNodeFunction findNode)
    : IIconNode, IRawIconNode
{
    public static DefaultIconNodeAdapter? Create(Wz_Node node, GlobalFindNodeFunction findNode)
    {
        var infoNode = node.Nodes["info"];
        return infoNode == null ? null : new DefaultIconNodeAdapter(node, infoNode, findNode);
    }

    public string Id => node.Text.Split(".")[0].TrimStart('0');

    public string IconId => Id;

    public Bitmap? Icon => IconUtility.GetIconImage(infoNode, "icon", findNode);

    public Point? IconOrigin => IconUtility.GetIconOrigin(infoNode, "icon", findNode);

    public string RawIconId => Id;

    public Bitmap? RawIcon => IconUtility.GetIconImage(infoNode, "iconRaw", findNode);

    public Point? RawIconOrigin => IconUtility.GetIconOrigin(infoNode, "iconRaw", findNode);
}