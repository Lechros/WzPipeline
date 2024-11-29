using WzComparerR2.WzLib;
using WzJson.Common;

namespace WzJson.Soul;

public class StringSkillNodeRepository : INodeRepository
{
    private const string StringSkillNode = @"String\Skill.img";

    private readonly IWzProvider wzProvider;

    public StringSkillNodeRepository(IWzProvider wzProvider)
    {
        this.wzProvider = wzProvider;
    }

    public IEnumerable<Wz_Node> GetNodes()
    {
        var stringSkillNode = GetStringSkillNode();
        foreach (var skillNode in stringSkillNode.Nodes)
        {
            yield return skillNode;
        }

        stringSkillNode.GetNodeWzImage()?.Unextract();
    }

    private Wz_Node GetStringSkillNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(StringSkillNode, true)
               ?? throw new ApplicationException("Cannot find String Skill node at: " + StringSkillNode);
    }
}