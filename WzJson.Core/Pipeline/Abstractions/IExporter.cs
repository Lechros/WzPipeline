namespace WzJson.Core.Pipeline.Abstractions;

public interface IExporter
{
    public void Export(IEnumerable<object> models, string path);
}

public interface IExporter<in T>
{
    public void Export(IEnumerable<T> models, string path);
}