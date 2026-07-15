using WzPipeline.Domains.Shared.String;
using WzPipeline.Shared;

namespace WzPipeline.Application.Gear;

public class GearStringDataProvider(GearStringDataBuilder builder)
    : AsyncDataProvider<Dictionary<string, NameDesc>>
{
    protected override Task<Dictionary<string, NameDesc>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}