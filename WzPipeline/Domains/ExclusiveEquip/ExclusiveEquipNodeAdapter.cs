using WzComparerR2.WzLib;

namespace WzPipeline.Domains.ExclusiveEquip;

public class ExclusiveEquipNodeAdapter(Wz_Node node) : IExclusiveEquipNode
{
    public static ExclusiveEquipNodeAdapter Create(Wz_Node node)
    {
        return new ExclusiveEquipNodeAdapter(node);
    }

    public string Id => node.Text;
    public string? Info => node.Nodes["info"]?.GetValue<string>();
    public int[] ItemIds => node.Nodes["item"].Nodes.Select(n => n.GetValue<int>()).ToArray();
    public string? Msg => node.Nodes["msg"]?.GetValue<string>();
}