using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Data;
using WzJson.DataProvider;

namespace WzJson.Converter;

public class SkillOptionConverter(
    SoulCollectionDataProvider soulCollectionDataProvider,
    ItemOptionDataProvider itemOptionDataProvider)
    : AbstractNodeConverter<SkillOptionNode>
{
    private const int NormalSoulReqLevel = 75;
    private const int MagnificentSoulReqLevel = 100;

    public override string GetNodeKey(Wz_Node node) => node.Text;

    public override SkillOptionNode? Convert(Wz_Node node, string key)
    {
        var skillOptionNode = new SkillOptionNode
        {
            SkillId = node.Nodes["skillId"].GetValue<int>(),
            ReqLevel = node.Nodes["reqLevel"].GetValue<int>(),
            IncTableId = node.Nodes["incTableID"]?.GetValue<int>() ?? 0,
        };

        if (!soulCollectionDataProvider.Data.ContainsSkill(skillOptionNode.SkillId)) return null;

        var magnificent = soulCollectionDataProvider.Data.IsMagnificentSoulSkill(skillOptionNode.SkillId);
        var expectedReqLevel = magnificent ? MagnificentSoulReqLevel : NormalSoulReqLevel;
        if (skillOptionNode.ReqLevel != expectedReqLevel) return null;

        foreach (var optionNode in node.Nodes["tempOption"].Nodes)
        {
            var id = optionNode.Nodes["id"].GetValue<int>();
            var gearOption = itemOptionDataProvider.Data.GetGearOptionByReqLevel(id, skillOptionNode.ReqLevel);
            skillOptionNode.TempOption.Add(gearOption);
        }

        return skillOptionNode;
    }
}