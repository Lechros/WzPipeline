namespace WzJson.Core.Abstractions;

public abstract class AbstractExporter<T> : IExporter<T>, IExporter
{
    public abstract void Export(IEnumerable<T> models, string path);

    public void Export(IEnumerable<object> models, string path)
    {
        Export(models.Cast<T>(), path);
    }
}