using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SoulCollectionNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => @"Etc\SoulCollection.img";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        var rootNode = GetRootNode();
        foreach (var itemNode in rootNode.Nodes)
        {
            yield return itemNode;
        }

        rootNode.GetNodeWzImage()?.Unextract();
    }
}