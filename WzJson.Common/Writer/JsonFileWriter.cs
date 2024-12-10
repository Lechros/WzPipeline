using System.Collections;
using Newtonsoft.Json;
using WzJson.Common.Data;

namespace WzJson.Common.Writer;

public class JsonFileWriter(string outputPath, JsonSerializer serializer) : AbstractFileWriter(outputPath)
{
    public override bool Supports(IData data)
    {
        var type = data.GetType();
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(JsonData<>);
    }

    protected override void WriteItems(IData data, IProgress<WriteProgressData> progress)
    {
        var dictData = (IKeyValueData)data;
        var sortedItems = ToSortedItems(dictData);

        var reporter = new ProgressReporter<WriteProgressData>(progress,
            (current, total) => new WriteProgressData(current, total),
            sortedItems.Count);

        dynamic jsonData = dictData;
        var filename = Path.Join(OutputPath, jsonData.Path);
        EnsureDirectory(filename);

        using StreamWriter sw = new(filename);
        using JsonWriter writer = new JsonTextWriter(sw);
        serializer.Serialize(writer, sortedItems);

        reporter.Complete();
    }

    private SortedDictionary<string, object?> ToSortedItems(IKeyValueData dictData)
    {
        var areAllKeysParsableAsInt = dictData.Keys.Cast<string>().All(key => int.TryParse(key, out _));
        var comparer = GetKeyComparer(areAllKeysParsableAsInt);
        var sortedItems = new SortedDictionary<string, object?>(comparer);
        foreach (dynamic kvp in dictData)
        {
            sortedItems.Add(kvp.Key, kvp.Value);
        }

        return sortedItems;
    }

    private IComparer<string> GetKeyComparer(bool areAllKeysParsableAsInt)
    {
        return areAllKeysParsableAsInt
            ? Comparer<string>.Create((a, b) => int.Parse(a).CompareTo(int.Parse(b)))
            : new NaturalStringComparer();
    }
}