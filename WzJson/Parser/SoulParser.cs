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
        var nameDescData = new NameDescConverter().Convert(soulStringNodeRepository.GetNodes());
        var soulSkillNameData = new NameDescConverter().Convert(stringSkillNodeRepository.GetNodes());
        return [new SoulConverter(SoulDataJsonPath, nameDescData, soulSkillNameData)];
    }
}