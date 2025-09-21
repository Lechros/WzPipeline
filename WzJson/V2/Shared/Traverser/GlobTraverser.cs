using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.V2.Core.Stereotype;
using WzJson.V2.Shared.Traverser.Internal;

namespace WzJson.V2.Shared.Traverser;

public class GlobTraverser<TNode>(IWzProvider wzProvider, string path, Func<Wz_Node, TNode?> factory)
    : AbstractTraverser<TNode> where TNode : INode
{
    private readonly GlobNodeWalker _walker = new(wzProvider, path);

    public override IEnumerable<TNode> EnumerateNodes()
    {
        return _walker.GetNodes().Select(factory).Where(n => n != null).Select(n => n!);
    }

    public override int GetNodeCount()
    {
        return _walker.GetNodeCount();
    }
}