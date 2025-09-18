using WzJson.V2.Core.Stereotype;

namespace WzJson.V2.Core.Pipeline;

public interface IProcessorNode : IGraphNode
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}