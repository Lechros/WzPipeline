namespace WzJson.Common.V2.Graph;

public interface IExporterNode : IGraphNode
{
    public IExporter Exporter { get; }

    public string Path { get; }
}