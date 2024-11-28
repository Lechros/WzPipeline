namespace WzJson;

public interface IData
{
    public void Add<T>(string name, T item) where T : notnull;
}