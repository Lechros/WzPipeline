namespace WzJson.V2.Core.Pipeline;

public class RootNode(string name) : IPipelineNode
{
    public string Name => name;
    public PipelineNodeType Type => PipelineNodeType.Default;
    public IPipelineNode? Parent => null;
    public IList<IPipelineNode> Children { get; } = [];
}