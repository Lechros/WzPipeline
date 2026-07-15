using WzPipeline.Domains.Soul;
using WzPipeline.Shared;

namespace WzPipeline.Application.Soul;

public class SoulInfoDataProvider(SoulInfoDataBuilder builder) : AsyncDataProvider<Dictionary<int, SoulInfo>>
{
    protected override Task<Dictionary<int, SoulInfo>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}