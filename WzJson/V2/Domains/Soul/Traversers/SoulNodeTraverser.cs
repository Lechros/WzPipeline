using WzJson.Common;
using WzJson.V2.Domains.Soul.Nodes;
using WzJson.V2.Shared.Traverser;

namespace WzJson.V2.Domains.Soul.Traversers;

public class SoulNodeTraverser(IWzProvider wzProvider)
    : GlobTraverser<ISoulNode>(wzProvider, "Item/Consume/0259.img/*", SoulNodeAdapter.Create);