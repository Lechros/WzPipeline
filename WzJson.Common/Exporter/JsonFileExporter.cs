using System.Collections.Immutable;
using Newtonsoft.Json;
using WzJson.Common.Data;

namespace WzJson.Common.Exporter;

public class JsonFileExporter(string outputPath, JsonSerializer serializer) : AbstractFileExporter(outputPath)
{
    public override bool Supports(IData data)
    {
        return data is JsonData;
    }

    protected override void ExportItems(IData data)
    {
        var jsonData = (JsonData)data;
        var sortedItems = jsonData.Items.ToImmutableSortedDictionary(new NaturalStringComparer());

        var filename = Path.Join(OutputPath, jsonData.Path);
        EnsureDirectory(filename);

        using StreamWriter sw = new(filename);
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, sortedItems);
    }
}