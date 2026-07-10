using WzPipeline.OldDomains.Soul.Nodes;
using WzPipeline.Shared;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.OldDomains.Soul.Traversers;

public class SoulNodeTraverser(IWzProvider wzProvider)
    : GlobTraverser<ISoulNode>(wzProvider, "Item/Consume/0259.img/*", SoulNodeAdapter.Create);