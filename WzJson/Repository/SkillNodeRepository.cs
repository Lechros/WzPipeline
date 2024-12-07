using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Repository;

public class SkillNodeRepository(IWzProvider wzProvider) : AbstractNodeRepository(wzProvider)
{
    protected override string RootNodePath => "Skill";

    public override IEnumerable<Wz_Node> GetNodes()
    {
        foreach (var jobNode in GetRootNode().Nodes)
        {
            if (!IsJobSkillNode(jobNode)) continue;

            var wzImage = jobNode.GetNodeWzImage();
            if (wzImage == null || !wzImage.TryExtract()) continue;

            var skillListNode = wzImage.Node.Nodes["skill"];
            foreach (var skillNode in skillListNode.Nodes)
            {
                yield return skillNode;
            }

            wzImage.Unextract();
        }
    }

    private bool IsJobSkillNode(Wz_Node node)
    {
        return char.IsDigit(node.Text[0]);
    }
}