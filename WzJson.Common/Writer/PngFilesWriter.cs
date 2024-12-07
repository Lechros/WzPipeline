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

    protected override void WriteItems(IData data, IProgress<WriteProgressData> progress)
    {
        var bitmapData = (BitmapData)data;
        var items = bitmapData.Items;

        var reporter = new ProgressReporter<WriteProgressData>(progress,
            (current, total) => new WriteProgressData(current, total),
            bitmapData.Items.Count);

        var rootPath = Path.Join(OutputPath, bitmapData.Path);

        Parallel.ForEach(items, e =>
        {
            var (key, bitmap) = e;
            var filename = Path.Join(rootPath, key);
            SavePng(bitmap, filename);

            reporter.Increment();
        });
        
        reporter.Complete();
    }

    private void SavePng(Bitmap bitmap, string filename)
    {
        EnsureDirectory(filename);
        bitmap.Save(filename, ImageFormat.Png);
    }
}