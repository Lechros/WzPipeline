using WzPipeline.Shared;

namespace WzPipeline.Application.Skill;

public class SkillNameDataProvider(SkillNameDataBuilder builder) : AsyncDataProvider<Dictionary<string, string>>
{
    protected override Task<Dictionary<string, string>> CreateAsync()
    {
        return builder.BuildAsync();
    }
}