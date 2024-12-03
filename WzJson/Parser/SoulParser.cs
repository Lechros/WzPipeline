using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Parser;

public class SoulParser(
    SoulNodeRepository soulNodeRepository,
    SoulStringNodeRepository soulStringNodeRepository,
    StringSkillNodeRepository stringSkillNodeRepository)
    : AbstractWzParser
{
    public const string SoulDataJsonPath = "soul-data.json";

    protected override IEnumerable<Wz_Node> GetNodes() => soulNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        var wzStringData = WzStringConverter.Instance.Convert(soulStringNodeRepository.GetNodes());
        var soulSkillNameData =
            WzStringConverter.Instance.Convert(stringSkillNodeRepository.GetNodes(), GetSkillNodeName);
        return [new SoulConverter(SoulDataJsonPath, wzStringData, soulSkillNameData)];
    }

    private string GetSkillNodeName(Wz_Node node)
    {
        return node.Text.TrimStart('0');
    }
}