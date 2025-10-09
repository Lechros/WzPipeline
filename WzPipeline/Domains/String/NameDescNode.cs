using WzComparerR2.WzLib;

namespace WzPipeline.Domains.String;

public class NameDescNode(Wz_Node node) : INameDescNode
{
    public static NameDescNode Create(Wz_Node node)
    {
        return new NameDescNode(node);
    }
    
    public string Id => node.Text;
    public string? Name => node.Nodes["name"]?.GetValue<string>();
    public string? Desc => node.Nodes["desc"]?.GetValue<string>();
}