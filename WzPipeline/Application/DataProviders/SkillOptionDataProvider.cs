using WzPipeline.Application.DataBuilders;
using WzPipeline.Domains.Soul;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class SkillOptionDataProvider(SkillOptionDataBuilder builder)
    : AsyncDataProvider<Dictionary<int, IList<SkillOption>>>
{
    protected override Task<Dictionary<int, IList<SkillOption>>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}