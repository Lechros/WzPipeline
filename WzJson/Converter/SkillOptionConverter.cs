using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Data;
using WzJson.Model;

namespace WzJson.Converter;

public class SkillOptionConverter(SoulCollectionData soulCollectionData, JsonData<ItemOption> itemOptionData)
    : AbstractNodeConverter<SkillOptionNode>
{
    private const int NormalSoulReqLevel = 75;
    private const int MagnificentSoulReqLevel = 100;

    public override IKeyValueData NewData() => new SkillOptionData();

    public override string GetNodeKey(Wz_Node node) => node.Text;

    public override SkillOptionNode? ConvertNode(Wz_Node node, string key)
    {
        var skillOptionNode = new SkillOptionNode
        {
            SkillId = node.Nodes["skillId"].GetValue<int>(),
            ReqLevel = node.Nodes["reqLevel"].GetValue<int>(),
            IncTableId = node.Nodes["incTableID"]?.GetValue<int>() ?? 0,
        };

        if (!soulCollectionData.ContainsSkill(skillOptionNode.SkillId)) return null;

        var magnificent = soulCollectionData.IsMagnificentSoulSkill(skillOptionNode.SkillId);
        var expectedReqLevel = magnificent ? MagnificentSoulReqLevel : NormalSoulReqLevel;
        if (skillOptionNode.ReqLevel != expectedReqLevel) return null;

        foreach (var optionNode in node.Nodes["tempOption"].Nodes)
        {
            var id = optionNode.Nodes["id"].GetValue<string>();
            var gearOption = itemOptionData[id].Level[GetLevel(skillOptionNode.ReqLevel)].Option;
            skillOptionNode.TempOption.Add(gearOption);
        }

        return skillOptionNode;
    }

    private int GetLevel(int reqLevel)
    {
        return reqLevel switch
        {
            <= 0 => 1,
            >= 250 => 25,
            _ => (reqLevel + 9) / 10
        };
    }
}