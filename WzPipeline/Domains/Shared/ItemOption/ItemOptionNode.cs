using System.Globalization;
using Wz;
using WzPipeline.MapleData;

namespace WzPipeline.Domains.Shared.ItemOption;

public class ItemOptionNode(IWzNode node)
{
    private IWzNode InfoNode => node.Nodes.Find("info") ??
                                throw new InvalidDataException(
                                    $"info node not found for item option {node.GetFullPath()}");

    private IWzNode LevelNode => node.Nodes.Find("level") ??
                                 throw new InvalidDataException(
                                     $"level node not found for item option {node.GetFullPath()}");

    public string Id => node.Name.Split('.')[0].TrimStart('0');
    public int? OptionType => InfoNode.Nodes.Find("optionType")?.GetInt32();
    public int? ReqLevel => InfoNode.Nodes.Find("reqLevel")?.GetInt32();
    public string String => InfoNode.Nodes["string"].GetString()!;

    public (int, Dictionary<string, string>)[] LevelOptions => LevelNode.Nodes.Select(level =>
        (int.Parse(level.Name), level.Nodes.ToDictionary(n => n.Name, GetLevelOptionValue))).ToArray();

    private static string GetLevelOptionValue(IWzNode node)
    {
        if (node.Type == WzNodeType.Single)
            return node.GetSingle().ToString(CultureInfo.InvariantCulture);
        if (node.Type == WzNodeType.Double)
            return node.GetDouble().ToString(CultureInfo.InvariantCulture);

        if (node.TryConvertString(out var value))
            return value;

        if (node.Type == WzNodeType.Null)
            return null!;

        throw new InvalidDataException(
            $"Item option value '{node.GetFullPath()}' of type '{node.Type}' is not scalar.");
    }
}
