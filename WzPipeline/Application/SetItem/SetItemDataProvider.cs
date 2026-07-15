using WzPipeline.Domains.SetItem;
using WzPipeline.Shared;

namespace WzPipeline.Application.SetItem;

class SetItemDataProvider(SetItemDataBuilder builder) : AsyncDataProvider<SortedDictionary<int, MalibSetItem>>
{
    protected override Task<SortedDictionary<int, MalibSetItem>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}