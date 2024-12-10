namespace WzJson.Common;

public interface INodeProcessor<out TItem, out TKeyValueData> : INodeProcessor
    where TKeyValueData : IKeyValueData<TItem>
{
    public new INodeConverter<TItem> Converter { get; }

    public new TKeyValueData CreateData();
}

public interface INodeProcessor
{
    public INodeConverter Converter { get; }

    public IKeyValueData CreateData();
}