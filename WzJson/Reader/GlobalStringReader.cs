using WzComparerR2.WzLib;
using WzJson.Common.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public sealed class GlobalStringReader(
    StringConsumeNodeRepository stringConsumeNodeRepository,
    StringEqpNodeRepository stringEqpNodeRepository,
    StringSkillNodeRepository stringSkillNodeRepository)
{
    public GlobalStringData Read()
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