using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Skill;

public class SkillNodeRepository : INodeRepository
{
    private const string SkillNodePath = "Skill";

    private readonly IWzProvider wzProvider;
    private readonly Wz_Node skillRootNode;

    public SkillNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
        skillRootNode = GetSkillNode();
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        foreach (var jobNode in skillRootNode.Nodes)
        {
            if (!IsJobSkillNode(jobNode)) continue;

            var wzImage = jobNode.GetNodeWzImage();
            if (wzImage == null || !wzImage.TryExtract()) continue;

            var skillListNode = wzImage.Node.FindNodeByPath("skill")!;
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

    private Wz_Node GetSkillNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(SkillNodePath)
               ?? throw new ApplicationException("Cannot find Skill node at: " + SkillNodePath);
    }
}