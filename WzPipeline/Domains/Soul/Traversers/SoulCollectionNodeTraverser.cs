using WzPipeline.Domains.Soul.Nodes;
using WzPipeline.Shared;
using WzPipeline.Shared.Traverser;

namespace WzPipeline.Domains.Soul.Traversers;

public class SoulCollectionNodeTraverser(IWzProvider wzProvider)
    : GlobTraverser<ISoulCollectionNode>(wzProvider, "Etc/SoulCollection.img/*", SoulCollectionNodeAdapter.Create);