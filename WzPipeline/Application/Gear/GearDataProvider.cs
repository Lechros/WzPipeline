using WzPipeline.Domains.Gear;
using WzPipeline.Shared;

namespace WzPipeline.Application.Gear;

public class GearDataProvider(GearDataBuilder builder) : AsyncDataProvider<SortedDictionary<int, MalibGear>>
{
    protected override Task<SortedDictionary<int, MalibGear>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}