using WzComparerR2.WzLib;
using WzJson.ItemOption;

namespace WzJson.SetItem;

public class SetItemParser : AbstractWzParser
{
    public const string SetItemJsonName = "set-item.json";

    private readonly SetItemNodeRepository setItemNodeRepository;
    private readonly ItemOptionNodeRepository itemOptionNodeRepository;

    public SetItemParser(SetItemNodeRepository setItemNodeRepository, ItemOptionNodeRepository itemOptionNodeRepository)
    {
        this.setItemNodeRepository = setItemNodeRepository;
        this.itemOptionNodeRepository = itemOptionNodeRepository;
    }

    protected override IEnumerable<Wz_Node> GetNodes() => setItemNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        var itemOptionData = new ItemOptionConverter(string.Empty).Convert(itemOptionNodeRepository.GetNodes());
        return new List<INodeConverter<object>>
        {
            new SetItemConverter(SetItemJsonName, itemOptionData)
        };
    }
}