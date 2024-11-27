namespace WzJson;

public interface IExporter
{
    public bool Supports<T>(IData<T> data);
    public void Export<T>(IData<T> data);
}