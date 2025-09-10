namespace WzJson.Common.V2.Dsl;

interface IExporterChainable<out TThis, TExporterIn>
{
    public IEnumerable<IExporterBuilder<TExporterIn>> ChainedExporters { get; }

    public TThis Exporter(IExporter<TExporterIn> next, string path);

    public TThis Exporter(Condition when, IExporter<TExporterIn> next, string path);
}