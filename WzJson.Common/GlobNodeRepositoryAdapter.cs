using WzComparerR2.WzLib;
using WzJson.Common.WzGlob;

namespace WzJson.Common;

public class GlobNodeRepositoryAdapter(IWzProvider wzProvider, string pattern) : INodeRepository
{
    private readonly GlobNodeWalker walker = new(wzProvider, pattern);

    public IEnumerable<Wz_Node> GetNodes()
    {
        return walker.GetNodes();
    }

    public int GetNodeCount()
    {
        return walker.GetNodeCount();
    }
}