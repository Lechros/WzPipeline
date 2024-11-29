using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson.Common;

public interface IWzProvider
{
    public Wz_Node BaseNode { get; }
    public GlobalFindNodeFunction FindNodeFunction { get; }
}