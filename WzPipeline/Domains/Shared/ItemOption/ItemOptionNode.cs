using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Shared.ItemOption;

public class ItemOptionNode(Wz_Node node)
{
    private Wz_Node InfoNode => node.Nodes["info"] ??
                                throw new InvalidDataException(
                                    $"info node not found for item option {node.FullPath}");

    private Wz_Node LevelNode => node.Nodes["level"] ??
                                 throw new InvalidDataException(
                                     $"level node not found for item option {node.FullPath}");

    public string Id => node.Text.Split(".")[0].TrimStart('0');
    public int? OptionType => InfoNode.Nodes["optionType"]?.GetValue<int>();
    public int? ReqLevel => InfoNode.Nodes["reqLevel"]?.GetValue<int>();
    public string String => InfoNode.Nodes["string"].GetValue<string>();

    public (int, Dictionary<string, string>)[] LevelOptions => LevelNode.Nodes.Select(level =>
        (int.Parse(level.Text), level.Nodes.ToDictionary(n => n.Text, n => n.GetValue<string>()))).ToArray();
}