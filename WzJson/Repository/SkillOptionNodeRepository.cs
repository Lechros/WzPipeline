using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SkillOptionNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => @"Item\SkillOption.img\skill";

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