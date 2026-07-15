using Newtonsoft.Json;

namespace WzPipeline.Application.Shared.Export;

public class JsonFileExporter(JsonSerializer serializer)
{
    public async Task ExportAsync<T>(T value, string filename, CancellationToken cancellationToken = default)
    {
        var directory = Path.GetDirectoryName(filename);
        if (directory != null) Directory.CreateDirectory(directory);

        await using var stream = File.Create(filename);
        await using var writer = new StreamWriter(stream);
        serializer.Serialize(writer, value);
    }
}