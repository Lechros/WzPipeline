using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Shared.Export;

namespace WzPipeline.Application.Shared.Dataflow;

public static class DataflowExporters
{
    public static ITargetBlock<KeyValuePair<string, Bitmap>> ImageExporter(ImageFileExporter exporter,
        string outputPath)
    {
        return ImageExporter(exporter, outputPath, ImageFormat.Png);
    }

    public static ITargetBlock<KeyValuePair<string, Bitmap>> ImageExporter(ImageFileExporter exporter,
        string filename, ImageFormat format)
    {
        return new ActionBlock<KeyValuePair<string, Bitmap>>(async icon =>
        {
            using var image = icon.Value;
            var path = Path.Combine(filename, $"{icon.Key}.{format.ToString().ToLower()}");
            await exporter.ExportImageAsync(image, path);
        });
    }

    public static ITargetBlock<object> JsonExporter(JsonFileExporter exporter, string outputPath)
    {
        return new ActionBlock<object>(async model => { await exporter.ExportAsync(model, outputPath); });
    }
}