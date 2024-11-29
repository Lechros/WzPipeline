using WzComparerR2.WzLib;

namespace WzJson.Common;

public abstract class AbstractNodeRepository(IWzProvider wzProvider) : INodeRepository
{
    protected abstract string RootNodePath { get; }

    public abstract IEnumerable<Wz_Node> GetNodes();

    protected Wz_Node GetRootNode()
    {
        return wzProvider.BaseNode.FindNodeByPath(RootNodePath, true) ??
               throw new ApplicationException("Node not found at: " + RootNodePath);
    }
}