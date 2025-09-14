namespace WzJson.Common.V2.Graph;

public interface IConverterNode : IGraphNode
{
    public IConverter Converter { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}