using WzJson.Core.Pipeline.Abstractions;

namespace WzJson.Core.Pipeline.Graph;

public interface IProcessorNode : IGraphNode
{
    public IProcessor Processor { get; }

    public void AddChild(IProcessorNode node);
    public void AddChild(IExporterNode node);
}