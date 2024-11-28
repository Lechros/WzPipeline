using WzComparerR2.WzLib;

namespace WzJson.Item;

public class ItemNodeRepository : INodeRepository
{
    private const string ItemNodePath = "Item";

    private static readonly ISet<string> PartNames = new HashSet<string>
    {
        "Cash", "Consume", "Etc"
    };

    private readonly IWzProvider wzProvider;
    private readonly Wz_Node itemRootNode;

    public ItemNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
        itemRootNode = GetItemNode();
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        foreach (var partNode in itemRootNode.Nodes)
        {
            if (!PartNames.Contains(partNode.Text)) continue;
            foreach (var itemListNode in partNode.Nodes)
            {
                var wzImage = itemListNode.GetNodeWzImage();
                if (wzImage == null || !wzImage.TryExtract()) continue;

                foreach (var itemNode in wzImage.Node.Nodes)
                {
                    yield return itemNode;
                }

                wzImage.Unextract();
            }
        }
    }

    private Wz_Node GetItemNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(ItemNodePath)
               ?? throw new ApplicationException("Cannot find Item node at: " + ItemNodePath);
    }
}