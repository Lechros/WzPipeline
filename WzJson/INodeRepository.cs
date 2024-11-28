using WzComparerR2.WzLib;

namespace WzJson;

public interface INodeRepository
{
    public IEnumerable<Wz_Node> GetNodes();
}