using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson.Common;

public static class WzUtility
{
    public static string GetNodeCode(Wz_Node node)
    {
        return node.Text.Split('.')[0].TrimStart('0');
    }

    public static Wz_Node ResolveLinkedNode(Wz_Node node, GlobalFindNodeFunction findNodeFunction)
    {
        Wz_Uol? uol;
        while ((uol = node.GetValue<Wz_Uol?>(null)) != null)
            node = uol.HandleUol(node);

        return node.GetLinkedSourceNode(findNodeFunction)!;
    }
}