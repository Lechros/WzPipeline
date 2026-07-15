using WzPipeline.Shared;

namespace WzPipeline.Application.Item;

public class ConsumeNameDataProvider(ConsumeNameDataBuilder builder) : AsyncDataProvider<Dictionary<string, string>>
{
    protected override Task<Dictionary<string, string>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}