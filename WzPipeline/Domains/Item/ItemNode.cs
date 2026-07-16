using WzComparerR2;
using WzComparerR2.WzLib;
using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Shared.Icon;

namespace WzPipeline.Domains.Item;

public class ItemNode(Wz_Node node)
{
    private Wz_Node InfoNode => node.Nodes["info"] ?? throw DataFormatException.MissingRequiredNode(node, "info");
    public string Id => node.Text;

    public IconNode? GetIconNode(GlobalFindNodeFunction findNode)
    {
        var iconNode = InfoNode.FindNodeByPath("icon");
        return iconNode is null ? null : IconNode.Create(Id?.ToString() ?? "(null)", iconNode, findNode);
    }

    public IconNode? GetIconRawNode(GlobalFindNodeFunction findNode)
    {
        var iconRawNode = InfoNode.FindNodeByPath("iconRaw");
        return iconRawNode is null ? null : IconNode.Create(Id?.ToString() ?? "(null)", iconRawNode, findNode);
    }
}