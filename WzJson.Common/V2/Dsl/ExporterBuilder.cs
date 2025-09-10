namespace WzJson.Common.V2.Dsl;

public class ExporterBuilder<TIn> : IExporterBuilder<TIn>
{
    private readonly IExporter<TIn> _exporter;

    internal ExporterBuilder(IExporter<TIn> exporter, string path)
    {
        _exporter = exporter;
        Path = path;
    }

    public IExporter<TIn> Get() => _exporter;

    public string Path { get; }
}