using WzComparerR2;
using WzComparerR2.WzLib;

namespace WzPipeline.Wz;

public static class WzImageExtensions
{
    public static bool TryExtractThreadSafe(this Wz_Image image)
    {
        lock (image.WzFile.ReadLock)
        {
            return image.TryExtract();
        }
    }

    public static bool TryExtractThreadSafe(this Wz_Image image, out Exception exception)
    {
        lock (image.WzFile.ReadLock)
        {
            return image.TryExtract(out exception);
        }
    }

    public static Wz_Node? FindNodeByPathThreadSafe(this Wz_Node node, params string[] fullPath)
    {
        var current = node;
        foreach (var segment in fullPath)
        {
            current = current.Nodes[segment];
            if (current == null)
            {
                return null;
            }

            var image = current.GetValueEx<Wz_Image?>(null);
            if (image == null)
            {
                continue;
            }

            current = image.TryExtractThreadSafe() ? image.Node : null;
            if (current == null)
            {
                return null;
            }
        }

        return current;
    }

    public static Wz_Node? GetLinkedSourceNodeThreadSafe(
        this Wz_Node node,
        GlobalFindNodeFunction findNode,
        Wz_File? wzFile = null)
    {
        string? path;

        if (!string.IsNullOrEmpty(path = node.Nodes["source"].GetValueEx<string?>(null)))
        {
            return findNode.Invoke(path, wzFile);
        }

        if (!string.IsNullOrEmpty(path = node.Nodes["_inlink"].GetValueEx<string?>(null)))
        {
            var image = node.GetNodeWzImage();
            return image?.Node.FindNodeByPathThreadSafe(path.Split('/'));
        }

        if (!string.IsNullOrEmpty(path = node.Nodes["_outlink"].GetValueEx<string?>(null)))
        {
            return findNode.Invoke(path, wzFile);
        }

        return node;
    }
}