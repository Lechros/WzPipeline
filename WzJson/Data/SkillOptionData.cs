using WzJson.Common;
using WzJson.Model;

namespace WzJson.Data;

public class SkillOptionData : AbstractKeyValueData<SkillOptionNode>
{
}

public class SkillOptionNode
{
    public int SkillId { get; set; }
    public int ReqLevel { get; set; }
    public int IncTableId { get; set; }
    public List<GearOption> TempOption { get; } = new();
}