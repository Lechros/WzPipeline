namespace WzJson.V2.Core.Pipeline;

public interface IPipelineNode
{
    public string Name { get; }
    public PipelineNodeType Type { get; }
    public IPipelineNode? Parent { get; }
    public IList<IPipelineNode> Children { get; }
}