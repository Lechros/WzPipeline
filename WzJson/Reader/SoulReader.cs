using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class SoulReader(
    SoulNodeRepository soulNodeRepository,
    GlobalStringData globalStringData)
    : AbstractWzReader
{
    public const string SoulDataJsonPath = "soul-data.json";

    protected override IEnumerable<Wz_Node> GetNodes() => soulNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters() =>
    [
        new SoulConverter(SoulDataJsonPath, globalStringData)
    ];
}