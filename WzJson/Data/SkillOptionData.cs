using WzJson.Common;
using WzJson.Model;

namespace WzJson.Data;

public class SkillOptionData : DefaultKeyValueData<SkillOptionNode>
{
    private readonly Dictionary<int, List<SkillOptionNode>> nodesBySkillId = new();

    public override SkillOptionNode this[string key]
    {
        set
        {
            base[key] = value;
            HandleAdd(key, value);
        }
    }

    public override void Add(string key, SkillOptionNode value)
    {
        base.Add(key, value);
        HandleAdd(key, value);
    }

    public List<SkillOptionNode> GetNodesBySkillId(int skillId)
    {
        return nodesBySkillId[skillId];
    }

    private void HandleAdd(string key, SkillOptionNode skillOptionNode)
    {
        if (skillOptionNode.IncTableId == 0) return;
        
        if (!nodesBySkillId.TryGetValue(skillOptionNode.SkillId, out var list))
        {
            list = [];
            nodesBySkillId.Add(skillOptionNode.SkillId, list);
        }

        list.Add(skillOptionNode);
    }
}

public class SkillOptionNode
{
    public int SkillId { get; set; }
    public int ReqLevel { get; set; }
    public int IncTableId { get; set; }
    public List<GearOption> TempOption { get; } = new();
}