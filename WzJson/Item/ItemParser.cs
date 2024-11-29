using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;

namespace WzJson.Item;

public class ItemParser : AbstractWzParser
{
    public const string ItemIconOriginJsonPath = "item-origin.json";
    public const string ItemIconPath = "item-icon";

    private readonly ItemNodeRepository itemNodeRepository;
    private readonly GlobalFindNodeFunction findNodeFunction;

    public ItemParser(ItemNodeRepository itemNodeRepository, GlobalFindNodeFunction findNodeFunction)
    {
        this.itemNodeRepository = itemNodeRepository;
        this.findNodeFunction = findNodeFunction;
    }

    protected override IEnumerable<Wz_Node> GetNodes() => itemNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        return new List<INodeConverter<object>>
        {
            new IconOriginConverter(ItemIconOriginJsonPath, @"info\icon\origin"),
            new IconBitmapConverter(ItemIconPath, @"info\icon", findNodeFunction)
        };
    }
}