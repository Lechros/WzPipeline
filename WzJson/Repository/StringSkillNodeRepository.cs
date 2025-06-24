using WzJson.Common;

namespace WzJson.Repository;

public class StringSkillNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "String/Skill.img/*")
{
}