namespace WzJson.Common.V2.Dsl;

public interface IExporterBuilder<in T>
{
    public IExporter<T> Get();

    public string Path { get; }
}