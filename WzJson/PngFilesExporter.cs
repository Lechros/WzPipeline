using System.Drawing;
using System.Drawing.Imaging;

namespace WzJson;

public class PngFilesExporter : AbstractFileExporter
{
    public PngFilesExporter(string outputPath) : base(outputPath)
    {
    }

    public override bool Supports<T>(IData<T> data)
    {
        return data is BitmapData;
    }

    protected override void ExportItems<T>(IData<T> data)
    {
        var items = (IReadOnlyDictionary<string, Bitmap>)data.Items;

        Parallel.ForEach(items, e =>
        {
            var (name, bitmap) = e;
            var filename = Path.Join(OutputPath, data.Name, name);
            SavePng(bitmap, filename);
        });
    }

    private void SavePng(Bitmap bitmap, string filename)
    {
        EnsureDirectory(filename);
        bitmap.Save(filename, ImageFormat.Png);
    }
}