using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class StringSkillNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => @"String\Skill.img";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        var rootNode = GetRootNode();
        foreach (var skillNode in rootNode.Nodes)
        {
            if (skillNode.Text.Length < 7) continue;

            yield return skillNode;
        }

        rootNode.GetNodeWzImage()?.Unextract();
    }
}