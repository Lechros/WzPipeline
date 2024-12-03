using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class SetItemReader(
    SetItemNodeRepository setItemNodeRepository,
    ItemOptionNodeRepository itemOptionNodeRepository) : AbstractWzReader
{
    public const string SetItemJsonName = "set-item.json";

    protected override IEnumerable<Wz_Node> GetNodes() => setItemNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        var itemOptionData = new ItemOptionConverter(string.Empty).Convert(itemOptionNodeRepository.GetNodes());
        return [new SetItemConverter(SetItemJsonName, itemOptionData)];
    }
}