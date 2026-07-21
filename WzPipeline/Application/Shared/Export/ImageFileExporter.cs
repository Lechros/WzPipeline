using System.Drawing;

namespace WzPipeline.Application.Shared.Export;

public class ImageFileExporter
{
    public Task ExportImageAsync(Image image, string filename, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var directory = Path.GetDirectoryName(filename);
        if (directory != null) Directory.CreateDirectory(directory);

        image.Save(filename);
        return Task.CompletedTask;
    }
}