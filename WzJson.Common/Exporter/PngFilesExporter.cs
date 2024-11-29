using System.Drawing;
using System.Drawing.Imaging;
using BitmapData = WzJson.Common.Data.BitmapData;

namespace WzJson.Common.Exporter;

public class PngFilesExporter(string outputPath) : AbstractFileExporter(outputPath)
{
    public override bool Supports(IData data)
    {
        return data is BitmapData;
    }

    protected override void ExportItems(IData data)
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