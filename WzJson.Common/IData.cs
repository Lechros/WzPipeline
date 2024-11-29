namespace WzJson.Common;

public interface IData
{
    public void Add<T>(string key, T item) where T : notnull;
}