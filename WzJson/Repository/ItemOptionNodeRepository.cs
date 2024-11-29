using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class ItemOptionNodeRepository : INodeRepository
{
    private const string ItemOptionNodePath = @"Item\ItemOption.img";

    private readonly IWzProvider wzProvider;

    public ItemOptionNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        var itemOptionNode = GetItemOptionNode();
        foreach (var optionNode in itemOptionNode.Nodes)
        {
            yield return optionNode;
        }

        itemOptionNode.GetNodeWzImage()?.Unextract();
    }

    private Wz_Node GetItemOptionNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(ItemOptionNodePath, true)
               ?? throw new ApplicationException("Cannot find String Eqp node at: " + ItemOptionNodePath);
    }
}