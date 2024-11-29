using WzComparerR2.WzLib;

namespace WzJson.Common;

public interface INodeRepository
{
    public IEnumerable<Wz_Node> GetNodes();
}