namespace WzJson;

public interface IData<T>
{
    public string Name { get; }
    public IDictionary<string, T> Items { get; }
}