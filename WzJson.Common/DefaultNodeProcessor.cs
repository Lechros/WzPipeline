using WzComparerR2.WzLib;

namespace WzJson.Common;

public class DefaultNodeProcessor<TItem, TKeyValueData>(
    INodeConverter<TItem> converter,
    Func<TKeyValueData> dataFactory)
    : INodeProcessor<TItem, TKeyValueData> where TKeyValueData : IKeyValueData<TItem>
{
    public INodeConverter<TItem> Converter { get; } = converter;

    public TKeyValueData CreateData() => dataFactory();

    public TKeyValueData ProcessNodes(IEnumerable<Wz_Node> nodes)
    {
        var pair = new ProcessUnit(Converter, CreateData());
        foreach (var node in nodes)
        {
            pair.ConvertNodeAndAdd(node);
        }

        return (TKeyValueData)pair.Data;
    }

    INodeConverter INodeProcessor.Converter => Converter;

    IKeyValueData INodeProcessor.CreateData() => CreateData();
}

public static class DefaultNodeProcessor
{
    public static DefaultNodeProcessor<TItem, TKeyValueData> Of<TItem, TKeyValueData>(INodeConverter<TItem> converter,
        Func<TKeyValueData> dataFactory) where TKeyValueData : IKeyValueData<TItem>
    {
        return new DefaultNodeProcessor<TItem, TKeyValueData>(converter, dataFactory);
    }
}

public readonly record struct ProcessUnit(INodeConverter Converter, IKeyValueData Data)
{
    public void ConvertNodeAndAdd(Wz_Node node)
    {
        var key = Converter.GetNodeKey(node);
        var item = Converter.Convert(node, key);
        if (item != null)
            Data.Add(key, item);
    }
}