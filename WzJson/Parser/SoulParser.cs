using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Parser;

public class SoulParser(
    SoulNodeRepository soulNodeRepository,
    GlobalStringData globalStringData)
    : AbstractWzParser
{
    public const string SoulDataJsonPath = "soul-data.json";

    protected override IEnumerable<Wz_Node> GetNodes() => soulNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters() =>
    [
        new SoulConverter(SoulDataJsonPath, globalStringData)
    ];
}