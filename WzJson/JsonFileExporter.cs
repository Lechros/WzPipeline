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

    public override bool Supports<T>(IData<T> data)
    {
        var type = data.GetType();
        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(JsonData<>))
            {
                return true;
            }

            type = type.BaseType;
        }

        return false;
    }

    protected override void ExportItems<T>(IData<T> data)
    {
        var sortedItems = data.Items.ToImmutableSortedDictionary();

        var filename = Path.Join(OutputPath, data.Name);
        EnsureDirectory(filename);

        using StreamWriter sw = new(filename);
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, sortedItems);
    }
}