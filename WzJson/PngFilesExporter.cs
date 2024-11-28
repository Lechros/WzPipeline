using System.Drawing;
using System.Drawing.Imaging;

namespace WzJson;

public class PngFilesExporter : AbstractFileExporter
{
    public PngFilesExporter(string outputPath) : base(outputPath)
    {
    }

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
            var (name, bitmap) = e;
            var filename = Path.Join(OutputPath, bitmapData.Path, name);
            SavePng(bitmap, filename);
        });
    }

    private void SavePng(Bitmap bitmap, string filename)
    {
        EnsureDirectory(filename);
        bitmap.Save(filename, ImageFormat.Png);
    }
}