using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Parser;

public class ItemOptionParser(ItemOptionNodeRepository itemOptionNodeRepository) : AbstractWzParser
{
    public const string ItemOptionJsonName = "item-option.json";

    protected override IEnumerable<Wz_Node> GetNodes() => itemOptionNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters() =>
    [
        new ItemOptionConverter(ItemOptionJsonName)
    ];
}