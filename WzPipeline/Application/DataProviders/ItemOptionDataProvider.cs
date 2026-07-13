using WzPipeline.Application.DataBuilders;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class ItemOptionDataProvider(ItemOptionDataBuilder builder) : AsyncDataProvider<ItemOptionData>
{
    protected override Task<ItemOptionData> CreateAsync()
    {
        return builder.BuildAsync();
    }
}