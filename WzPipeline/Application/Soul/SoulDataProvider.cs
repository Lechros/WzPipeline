using WzPipeline.Domains.Soul;
using WzPipeline.Shared;

namespace WzPipeline.Application.Soul;

public class SoulDataProvider(SoulDataBuilder builder) : AsyncDataProvider<SortedDictionary<int, MalibSoul>>
{
    protected override Task<SortedDictionary<int, MalibSoul>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}