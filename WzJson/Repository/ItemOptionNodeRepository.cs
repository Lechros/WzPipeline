using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class ItemOptionNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => @"Item\ItemOption.img";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        var rootNode = GetRootNode();
        foreach (var optionNode in rootNode.Nodes)
        {
            yield return optionNode;
        }

        rootNode.GetNodeWzImage()?.Unextract();
    }
}