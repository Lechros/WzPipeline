using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Parser;

public class SetItemParser(
    SetItemNodeRepository setItemNodeRepository,
    ItemOptionNodeRepository itemOptionNodeRepository) : AbstractWzParser
{
    public const string SetItemJsonName = "set-item.json";

    protected override IEnumerable<Wz_Node> GetNodes() => setItemNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        var itemOptionData = new ItemOptionConverter(string.Empty).Convert(itemOptionNodeRepository.GetNodes());
        return [new SetItemConverter(SetItemJsonName, itemOptionData)];
    }
}