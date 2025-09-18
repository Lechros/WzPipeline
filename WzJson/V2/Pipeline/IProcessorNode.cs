using WzJson.V2.Stereotype;

namespace WzJson.V2.Pipeline.Graph;

public interface IProcessorNode : IGraphNode
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}