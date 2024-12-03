using System.Collections;
using Newtonsoft.Json;
using WzJson.Common.Data;

namespace WzJson.Common.Writer;

public class JsonFileWriter(string outputPath, JsonSerializer serializer) : AbstractFileWriter(outputPath)
{
    public override bool Supports(IData data)
    {
        return data is JsonData;
    }

    protected override void WriteItems(IData data)
    {
        var jsonData = (JsonData)data;
        var sortedItems = ToSortedItems(jsonData);

        var filename = Path.Join(OutputPath, jsonData.Path);
        EnsureDirectory(filename);

        using StreamWriter sw = new(filename);
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, sortedItems);
    }

    private SortedDictionary<string, object> ToSortedItems(JsonData jsonData)
    {
        var comparer = GetKeyComparer(jsonData.Items);
        var sortedItems = new SortedDictionary<string, object>(comparer);
        foreach (DictionaryEntry entry in jsonData.Items)
            sortedItems.Add((string)entry.Key, entry.Value!);
        return sortedItems;
    }

    private IComparer<string> GetKeyComparer(IDictionary dictionary)
    {
        return AreAllKeysParsableAsInt(dictionary)
            ? Comparer<string>.Create((a, b) => int.Parse(a).CompareTo(int.Parse(b)))
            : new NaturalStringComparer();
    }

    private bool AreAllKeysParsableAsInt(IDictionary dictionary)
    {
        foreach (string key in dictionary.Keys)
        {
            if (!int.TryParse(key, out _)) return false;
        }
        return true;
    }
}