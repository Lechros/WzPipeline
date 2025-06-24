using WzJson.Common;

namespace WzJson.Repository;

public class SkillOptionNodeRepository(IWzProvider wzProvider)
    : GlobNodeRepositoryAdapter(wzProvider, "Item/SkillOption.img/skill/*")
{
}