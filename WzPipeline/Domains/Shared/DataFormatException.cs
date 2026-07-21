using WzComparerR2.WzLib;

namespace WzPipeline.Domains.Shared;

public class DataFormatException(string message) : Exception(message)
{
    public static DataFormatException MissingRequiredNode(Wz_Node node, string path)
    {
        return new DataFormatException($"Node not found: ParentNode={node.FullPathToFile}, Path={path}");
    }
}