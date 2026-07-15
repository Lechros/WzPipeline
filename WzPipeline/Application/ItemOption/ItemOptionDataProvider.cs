using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Shared;

namespace WzPipeline.Application.ItemOption;

public class ItemOptionDataProvider(ItemOptionDataBuilder builder) : AsyncDataProvider<ItemOptionData>
{
    protected override Task<ItemOptionData> CreateAsync()
    {
        return builder.BuildAsync();
    }
}