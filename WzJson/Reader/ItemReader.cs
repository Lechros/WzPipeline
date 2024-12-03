using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class ItemReader(ItemNodeRepository itemNodeRepository, GlobalFindNodeFunction findNode)
    : AbstractWzReader
{
    public const string ItemIconOriginJsonPath = "item-origin.json";
    public const string ItemIconPath = "item-icon";

    protected override IEnumerable<Wz_Node> GetNodes() => itemNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters() =>
    [
        new IconOriginConverter(ItemIconOriginJsonPath, @"info\icon\origin"),
        new IconBitmapConverter(ItemIconPath, @"info\icon", findNode)
    ];
}