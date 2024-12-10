using WzComparerR2.WzLib;
using WzJson.Common.Data;

namespace WzJson.Common.Converter;

public class IconOriginConverter(string originNodePath) : AbstractNodeConverter<int[]>
{
    public override string GetNodeKey(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override int[]? Convert(Wz_Node node, string _)
    {
        var originNode = node.FindNodeByPath(originNodePath);
        var vector = originNode?.GetValue<Wz_Vector?>();
        if (vector == null) return null;
        return new[] { vector.X, vector.Y };
    }
}