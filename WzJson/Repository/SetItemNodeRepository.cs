using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SetItemNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => @"Etc\SetItemInfo.img";
    
    public override IEnumerable<Wz_Node> GetNodes()
    {
        var rootNode = GetRootNode();
        foreach (var node in rootNode.Nodes)
        {
            yield return node;
        }

        rootNode.GetNodeWzImage()?.Unextract();
    }
}