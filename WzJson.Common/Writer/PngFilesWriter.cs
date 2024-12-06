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

        var total = bitmapData.Items.Count;
        var current = 0;
        progress.Report(new WriteProgressData(current, total));
        
        Parallel.ForEach(items, e =>
        {
            var (key, bitmap) = e;
            var filename = Path.Join(OutputPath, bitmapData.Path, key);
            SavePng(bitmap, filename);
            
            Interlocked.Increment(ref current);
            if (total < 100 || current % (total / 100) == 0)
                progress.Report(new WriteProgressData(current, total));
        });
        
        progress.Report(new WriteProgressData(total, total));
    }

    private void SavePng(Bitmap bitmap, string filename)
    {
        EnsureDirectory(filename);
        bitmap.Save(filename, ImageFormat.Png);
    }
}