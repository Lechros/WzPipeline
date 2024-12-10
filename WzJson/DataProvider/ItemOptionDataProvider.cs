using WzJson.Common.Data;
using WzJson.Converter;
using WzJson.Model;
using WzJson.Repository;

namespace WzJson.DataProvider;

public class ItemOptionDataProvider(ItemOptionNodeRepository itemOptionNodeRepository)
    : AbstractDataProvider<JsonData<ItemOption>>
{
    protected override JsonData<ItemOption> GetData()
    {
        return new ItemOptionConverter("", "").Convert(itemOptionNodeRepository.GetNodes());
    }
}