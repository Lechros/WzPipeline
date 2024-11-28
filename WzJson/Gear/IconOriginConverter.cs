using WzComparerR2.WzLib;

namespace WzJson.Gear;

public class IconOriginConverter : IDataConverter<JsonData>
{
    private readonly string dataName;
    private readonly string originNodePath;

    public IconOriginConverter(string dataName, string originNodePath)
    {
        this.dataName = dataName;
        this.originNodePath = originNodePath;
    }

    public JsonData Convert(IEnumerable<Wz_Node> nodes)
    {
        var data = new JsonData(dataName);
        foreach (var node in nodes)
        {
            var code = WzUtility.GetNodeCode(node);
            var origin = ConvertNode(node);
            if (origin != null)
                data.Items.Add(code, origin);
        }

        return data;
    }

    public int[]? ConvertNode(Wz_Node node)
    {
        var originNode = node.FindNodeByPath(originNodePath);
        var vector = originNode?.GetValue<Wz_Vector?>();
        if (vector == null) return null;
        return new[] { vector.X, vector.Y };
    }
}