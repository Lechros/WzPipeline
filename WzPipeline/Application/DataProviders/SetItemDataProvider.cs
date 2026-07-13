using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.DataBuilders;
using WzPipeline.Domains.SetItem;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

class SetItemDataProvider(SetItemDataBuilder builder) : AsyncDataProvider<SortedDictionary<int, MalibSetItem>>
{
    protected override Task<SortedDictionary<int, MalibSetItem>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}