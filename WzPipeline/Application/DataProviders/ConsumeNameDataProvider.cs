using WzPipeline.Application.DataBuilders;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class ConsumeNameDataProvider(ConsumeNameDataBuilder builder) : AsyncDataProvider<Dictionary<string, string>>
{
    protected override Task<Dictionary<string, string>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}