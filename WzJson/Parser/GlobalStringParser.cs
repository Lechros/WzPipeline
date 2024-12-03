using WzComparerR2.WzLib;
using WzJson.Common.Converter;
using WzJson.Repository;

namespace WzJson.Parser;

public sealed class GlobalStringParser(
    StringConsumeNodeRepository stringConsumeNodeRepository,
    StringEqpNodeRepository stringEqpNodeRepository,
    StringSkillNodeRepository stringSkillNodeRepository)
{
    public GlobalStringData Parse()
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