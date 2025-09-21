using Newtonsoft.Json;
using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Shared.Exporter;

public class DictionaryJsonWriter<TKey, TValue>(JsonSerializer serializer)
    : AbstractExporter<IDictionary<TKey, TValue>>
{
    public override void Export(IEnumerable<IDictionary<TKey, TValue>> models, string path)
    {
        var dict = models.Single();
        using var sw = new StreamWriter(path);
        using var writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, dict);
    }
}