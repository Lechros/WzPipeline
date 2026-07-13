using WzPipeline.Application.DataBuilders;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class GearStringDataProvider(GearStringDataBuilder builder)
    : AsyncDataProvider<Dictionary<string, NameDesc>>
{
    protected override Task<Dictionary<string, NameDesc>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}