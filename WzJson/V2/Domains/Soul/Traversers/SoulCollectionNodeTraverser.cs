using WzJson.Common;
using WzJson.V2.Domains.Soul.Nodes;
using WzJson.V2.Shared.Traverser;

namespace WzJson.V2.Domains.Soul.Traversers;

public class SoulCollectionNodeTraverser(IWzProvider wzProvider)
    : GlobTraverser<ISoulCollectionNode>(wzProvider, "Etc/SoulCollection.img/*", SoulCollectionNodeAdapter.Create);