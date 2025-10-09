using Newtonsoft.Json;
using WzPipeline.Core.Stereotype;

namespace WzPipeline.Shared.Exporter;

public class DictionaryJsonWriter<TKey, TValue>(JsonSerializer serializer, string filename)
    : AbstractExporter<IDictionary<TKey, TValue>>
{
    protected override void Prepare()
    {
        var directory = Path.GetDirectoryName(filename);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public override async Task Export(IDictionary<TKey, TValue> model)
    {
        await using var sw = new StreamWriter(filename);
        await using var writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, model);
        await writer.FlushAsync();
    }
}

public class DictionaryJsonWriterFactory(JsonSerializer serializer)
{
    public DictionaryJsonWriter<TKey, TValue> WithFilename<TKey, TValue>(string filename)
    {
        return new DictionaryJsonWriter<TKey, TValue>(serializer, filename);
    }
}