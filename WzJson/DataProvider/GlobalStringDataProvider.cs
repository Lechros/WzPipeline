using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Common.Data;
using WzJson.Data;
using WzJson.Repository;

namespace WzJson.DataProvider;

public sealed class GlobalStringDataProvider(
    StringConsumeNodeRepository stringConsumeNodeRepository,
    StringEqpNodeRepository stringEqpNodeRepository,
    StringSkillNodeRepository stringSkillNodeRepository,
    WzStringConverter wzStringConverter)
    : AbstractDataProvider<GlobalStringData>
{
    protected override GlobalStringData GetData()
    {
        var processor = DefaultNodeProcessor.Of(wzStringConverter, () => new WzStringData());
        return new GlobalStringData
        {
            Consume = processor.ProcessNodes(stringConsumeNodeRepository.GetNodes()),
            Eqp = processor.ProcessNodes(stringEqpNodeRepository.GetNodes()),
            Skill = processor.ProcessNodes(stringSkillNodeRepository.GetNodes())
        };
    }

    private string GetSkillNodeName(Wz_Node node)
    {
        return node.Text.TrimStart('0');
    }
}