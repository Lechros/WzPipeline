using WzJson.V2.Pipeline.Abstractions;

namespace WzJson.V2.Pipeline.Graph;

public interface IProcessorNode : IGraphNode
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}