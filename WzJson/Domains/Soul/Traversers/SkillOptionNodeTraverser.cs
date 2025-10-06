using WzJson.Domains.Soul.Nodes;
using WzJson.Shared;
using WzJson.Shared.Traverser;

namespace WzJson.Domains.Soul.Traversers;

public class SkillOptionNodeTraverser(IWzProvider wzProvider) : GlobTraverser<ISkillOptionNode>(wzProvider,
    "Item/SkillOption.img/skill/*", SkillOptionNodeAdapter.Create);