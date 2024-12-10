using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Converter;
using WzJson.Model;
using WzJson.Repository;

namespace WzJson.DataProvider;

public class ItemOptionDataProvider(
    ItemOptionNodeRepository itemOptionNodeRepository,
    ItemOptionConverter itemOptionConverter)
    : AbstractDataProvider<IKeyValueData<ItemOption>>
{
    protected override IKeyValueData<ItemOption> GetData()
    {
        var processor = DefaultNodeProcessor.Of(itemOptionConverter, () => new JsonData<ItemOption>("", ""));
        return processor.ProcessNodes(itemOptionNodeRepository.GetNodes());
    }
}