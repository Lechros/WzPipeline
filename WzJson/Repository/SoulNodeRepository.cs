using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SoulNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => @"Item\Consume\0259.img";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        var rootNode = GetRootNode();
        foreach (var soulNode in rootNode.Nodes)
        {
            yield return soulNode;
        }

        rootNode.GetNodeWzImage()?.Unextract();
    }
}