using WzComparerR2.WzLib;

namespace WzJson;

public class IconOriginConverter : INodeConverter<int[]>
{
    private readonly string dataName;
    private readonly string originNodePath;

    public IconOriginConverter(string dataName, string originNodePath)
    {
        this.dataName = dataName;
        this.originNodePath = originNodePath;
    }


    public IData NewData()
    {
        return new JsonData(dataName);
    }

    public string GetNodeName(Wz_Node node)
    {
        return WzUtility.GetNodeCode(node);
    }

    public int[]? ConvertNode(Wz_Node node, string _)
    {
        var originNode = node.FindNodeByPath(originNodePath);
        var vector = originNode?.GetValue<Wz_Vector?>();
        if (vector == null) return null;
        return new[] { vector.X, vector.Y };
    }
}