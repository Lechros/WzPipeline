using WzPipeline.Application.DataBuilders;
using WzPipeline.Domains.Gear;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class GearDataProvider(GearDataBuilder builder) : AsyncDataProvider<SortedDictionary<int, MalibGear>>
{
    protected override Task<SortedDictionary<int, MalibGear>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}