using WzComparerR2.WzLib;
using WzJson.Core.Stereotype;
using WzJson.Shared.Traverser.Internal;

namespace WzJson.Shared.Traverser;

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

public static class GlobTraverser
{
    public static GlobTraverser<TNode> Create<TNode>(IWzProvider wzProvider, string path, Func<Wz_Node, TNode?> factory)
        where TNode : INode
    {
        return new GlobTraverser<TNode>(wzProvider, path, factory);
    }
}