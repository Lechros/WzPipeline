using Newtonsoft.Json;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Shared.Exporter;

public class DictionaryJsonWriter<TKey, TValue>(JsonSerializer serializer, string outputPath)
    : AbstractExporter<IDictionary<TKey, TValue>>
{
    private readonly string _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));

    protected override void Prepare()
    {
        var directory = Path.GetDirectoryName(_outputPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public override async Task Export(IDictionary<TKey, TValue> model)
    {
        await using var sw = new StreamWriter(_outputPath);
        await using var writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, model);
        await writer.FlushAsync();
    }
}