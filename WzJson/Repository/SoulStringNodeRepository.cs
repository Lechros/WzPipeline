using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SoulStringNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => @"String\Consume.img";

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