using WzJson.Common;
using WzJson.Converter;
using WzJson.Data;
using WzJson.Repository;

namespace WzJson.DataProvider;

public class ItemOptionDataProvider(
    ItemOptionNodeRepository itemOptionNodeRepository,
    ItemOptionConverter itemOptionConverter)
    : AbstractDataProvider<ItemOptionData>
{
    protected override ItemOptionData GetData()
    {
        var processor = DefaultNodeProcessor.Of(itemOptionConverter, () => new ItemOptionData());
        return processor.ProcessNodes(itemOptionNodeRepository.GetNodes());
    }
}