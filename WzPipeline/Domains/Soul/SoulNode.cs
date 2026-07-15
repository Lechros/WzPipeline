using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Soul;

public class SoulNode(Wz_Node node)
{
    public string Id => node.Text.Split('.')[0].TrimStart('0');
}