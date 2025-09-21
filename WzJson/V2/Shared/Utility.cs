using System.Drawing.Imaging;
using SixLabors.ImageSharp.PixelFormats;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using Image = SixLabors.ImageSharp.Image;
using Bitmap = System.Drawing.Bitmap;
using Point = SixLabors.ImageSharp.Point;

namespace WzJson.V2.Shared;

public static class Utility
{
    public static Image? GetIconImage(Wz_Node node, string path, GlobalFindNodeFunction findNode)
    {
        var iconNode = node.FindNodeByPath(path);
        if (iconNode == null) return null;
        iconNode = iconNode.HandleFullUol(findNode).GetLinkedSourceNode(findNode);
        using var bitmap = iconNode.GetValue<Wz_Png>().ExtractPng();
        return bitmap.ToImage();
    }

    public static Point? GetIconOrigin(Wz_Node node, string path)
    {
        var originNode = node.FindNodeByPath(path);
        if (originNode == null) return null;
        var vector = originNode.GetValue<Wz_Vector>();
        return new Point(vector.X, vector.Y);
    }

    public static Image ToImage(this Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        stream.Position = 0;
        return Image.Load<Rgba32>(stream);
    }
}