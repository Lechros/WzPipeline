using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.SetItem;

public class SetItemNodeRepository : INodeRepository
{
    private const string SetItemInfoNodePath = @"Etc\SetItemInfo.img";

    private readonly IWzProvider wzProvider;

    public SetItemNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        var setItemInfoNode = GetSetItemInfoNode();
        foreach (var node in setItemInfoNode.Nodes)
        {
            yield return node;
        }

        setItemInfoNode.GetNodeWzImage()?.Unextract();
    }

    private Wz_Node GetSetItemInfoNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(SetItemInfoNodePath, true)
               ?? throw new ApplicationException("Cannot find SetItemInfo node at: " + SetItemInfoNodePath);
    }
}