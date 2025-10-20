using System.Text.Json;
using WzPipeline.Core.Stereotype;

namespace WzPipeline.Shared.Exporter;

public class JsonStringDictionaryWriter(string filename) : AbstractExporter<IDictionary<string, string>>
{
    public bool Indented { get; set; } = false;

    protected override void Prepare()
    {
        var directory = Path.GetDirectoryName(filename);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public override async Task Export(IDictionary<string, string> model)
    {
        var options = new JsonWriterOptions
        {
            Indented = Indented,
        };

        await using var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096,
            FileOptions.Asynchronous);
        await using var writer = new Utf8JsonWriter(fs, options);

        writer.WriteStartObject();

        foreach (var (key, json) in model)
        {
            writer.WritePropertyName(key);
            using var doc = JsonDocument.Parse(json);
            doc.RootElement.WriteTo(writer);
        }

        writer.WriteEndObject();
        await writer.FlushAsync();
    }
}

public class JsonStringDictionaryWriterFactory
{
    public JsonStringDictionaryWriter WithFilename(string filename)
    {
        return new JsonStringDictionaryWriter(filename);
    }

    public JsonStringDictionaryWriter IndentedWithFilename(string filename)
    {
        return new JsonStringDictionaryWriter(filename)
        {
            Indented = true
        };
    }
}