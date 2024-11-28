using System.Collections.Immutable;
using Newtonsoft.Json;

namespace WzJson;

public class JsonFileExporter : AbstractFileExporter
{
    private readonly JsonSerializer serializer;

    public JsonFileExporter(string outputPath, JsonSerializer serializer) : base(outputPath)
    {
        this.serializer = serializer;
    }

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