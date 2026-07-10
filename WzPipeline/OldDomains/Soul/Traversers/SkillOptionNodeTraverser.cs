using WzPipeline.OldDomains.Soul.Nodes;
using WzPipeline.Shared;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.OldDomains.Soul.Traversers;

public class SkillOptionNodeTraverser(IWzProvider wzProvider) : GlobTraverser<ISkillOptionNode>(wzProvider,
    "Item/SkillOption.img/skill/*", SkillOptionNodeAdapter.Create);