using WzComparerR2.WzLib;

namespace WzJson.Common;

public interface IWzProvider
{
    public Wz_Node BaseNode { get; }

    public Wz_Node? FindNode(string fullPath);
}