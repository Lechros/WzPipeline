using WzJson.Core.Abstractions;

namespace WzJson.Core.Pipeline;

public interface IProcessorNode : IGraphNode
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}