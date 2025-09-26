namespace WzJson.V2.Core.Pipeline;

public interface IPipelineNode
{
    public PipelineNodeType Type { get; }
    public IPipelineNode? Parent { get; }
    public IList<IPipelineNode> Children { get; }
}