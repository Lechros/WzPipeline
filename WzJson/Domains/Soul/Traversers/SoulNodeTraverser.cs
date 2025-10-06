using WzJson.Domains.Soul.Nodes;
using WzJson.Shared;
using WzJson.Shared.Traverser;

namespace WzJson.Domains.Soul.Traversers;

public class SoulNodeTraverser(IWzProvider wzProvider)
    : GlobTraverser<ISoulNode>(wzProvider, "Item/Consume/0259.img/*", SoulNodeAdapter.Create);