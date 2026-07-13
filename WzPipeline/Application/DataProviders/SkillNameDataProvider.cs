using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.DataBuilders;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class SkillNameDataProvider(SkillNameDataBuilder builder) : AsyncDataProvider<Dictionary<string, string>>
{
    protected override Task<Dictionary<string, string>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}