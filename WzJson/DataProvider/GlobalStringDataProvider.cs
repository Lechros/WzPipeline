using WzComparerR2.WzLib;
using WzJson.Common.Converter;
using WzJson.Data;
using WzJson.Repository;

namespace WzJson.DataProvider;

public sealed class GlobalStringDataProvider(
    StringConsumeNodeRepository stringConsumeNodeRepository,
    StringEqpNodeRepository stringEqpNodeRepository,
    StringSkillNodeRepository stringSkillNodeRepository)
    : AbstractDataProvider<GlobalStringData>
{
    protected override GlobalStringData GetData()
    {
        var converter = new WzStringConverter();
        return new GlobalStringData
        {
            Consume = converter.Convert(stringConsumeNodeRepository.GetNodes()),
            Eqp = converter.Convert(stringEqpNodeRepository.GetNodes()),
            Skill = converter.Convert(stringSkillNodeRepository.GetNodes(), GetSkillNodeName)
        };
    }

    private string GetSkillNodeName(Wz_Node node)
    {
        return node.Text.TrimStart('0');
    }
}