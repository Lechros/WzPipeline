using System.Drawing;
using System.Drawing.Imaging;
using BitmapData = WzJson.Common.Data.BitmapData;

namespace WzJson.Common.Writer;

public class PngFilesWriter(string outputPath) : AbstractFileWriter(outputPath)
{
    public override bool Supports(IData data)
    {
        return data is BitmapData;
    }

    protected override void WriteItems(IData data)
    {
        var bitmapData = (BitmapData)data;
        var items = bitmapData.Items;

        Parallel.ForEach(items, e =>
        {
            var (key, bitmap) = e;
            var filename = Path.Join(OutputPath, bitmapData.Path, key);
            SavePng(bitmap, filename);
        });
    }

    private void SavePng(Bitmap bitmap, string filename)
    {
        EnsureDirectory(filename);
        bitmap.Save(filename, ImageFormat.Png);
    }
}