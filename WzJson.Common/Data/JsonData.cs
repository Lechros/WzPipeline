namespace WzJson.Common.Data;

public sealed class JsonData<TItem>(string label, string path)
    : AbstractDictionaryData<TItem>(new Dictionary<string, TItem>()), ILabeled
{
    public string Label { get; } = label;
    public string Path { get; } = path;
}