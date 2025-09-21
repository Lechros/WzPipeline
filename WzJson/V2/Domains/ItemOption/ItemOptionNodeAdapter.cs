using WzComparerR2.WzLib;

namespace WzJson.V2.Domains.ItemOption;

public class ItemOptionNodeAdapter(Wz_Node node, Wz_Node infoNode, Wz_Node levelNode) : IItemOptionNode
{
    public static ItemOptionNodeAdapter Create(Wz_Node node)
    {
        var infoNode = node.Nodes["info"] ??
                       throw new InvalidDataException($"info node not found for item option {node.FullPath}");
        var levelListNode = node.Nodes["level"] ??
                            throw new InvalidDataException($"level node not found for item option {node.FullPath}");
        return new ItemOptionNodeAdapter(node, infoNode, levelListNode);
    }

    public string Id => node.Text.Split(".")[0].TrimStart('0');

    public int? OptionType => infoNode.Nodes["optionType"]?.GetValue<int>();

    public int? ReqLevel => infoNode.Nodes["reqLevel"]?.GetValue<int>();

    public string String => infoNode.Nodes["string"].GetValue<string>();

    public (int, Dictionary<string, string>)[] LevelOptions => node.Nodes["level"].Nodes.Select(level =>
        (int.Parse(level.Text), level.Nodes.ToDictionary(n => n.Text, n => n.GetValue<string>()))).ToArray();
}