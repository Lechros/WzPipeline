using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Shared.String;

public class StringNode(Wz_Node node)
{
    public string Key => node.Text;
    public string? Name => node.Nodes["name"]?.GetValue<string>();
    public string? Desc => node.Nodes["desc"]?.GetValue<string>();
}