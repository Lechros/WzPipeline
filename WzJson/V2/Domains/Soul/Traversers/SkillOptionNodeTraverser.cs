using WzJson.Common;
using WzJson.V2.Domains.Soul.Nodes;
using WzJson.V2.Shared.Traverser;

namespace WzJson.V2.Domains.Soul.Traversers;

public class SkillOptionNodeTraverser(IWzProvider wzProvider) : GlobTraverser<ISkillOptionNode>(wzProvider,
    "Item/SkillOption.img/skill/*", SkillOptionNodeAdapter.Create);