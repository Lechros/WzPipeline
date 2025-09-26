namespace WzJson.V2.Core.Pipeline;

public class RootNode : IPipelineNode
{
    public PipelineNodeType Type => PipelineNodeType.Default;
    public IPipelineNode? Parent => null;
    public IList<IPipelineNode> Children { get; } = [];
}