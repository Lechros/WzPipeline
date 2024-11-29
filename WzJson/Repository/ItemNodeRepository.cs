using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class ItemNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    private static readonly HashSet<string> PartNames = ["Cash", "Consume", "Etc"];

    protected override string RootNodePath => "Item";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        foreach (var partNode in GetRootNode().Nodes)
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
}