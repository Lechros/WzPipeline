using WzJson.Domains.Soul.Nodes;
using WzJson.Shared;
using WzJson.Shared.Traverser;

namespace WzJson.Domains.Soul.Traversers;

public class SoulCollectionNodeTraverser(IWzProvider wzProvider)
    : GlobTraverser<ISoulCollectionNode>(wzProvider, "Etc/SoulCollection.img/*", SoulCollectionNodeAdapter.Create);