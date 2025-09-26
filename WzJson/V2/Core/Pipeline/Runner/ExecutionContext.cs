namespace WzJson.V2.Core.Pipeline.Runner;

public class ExecutionContext
{
    public readonly List<ITraverserNode> TraverserNodes = [];
    public readonly Queue<(IProcessorNode, ICollection<object>)> ProcessorNodeQueue = new();
    public readonly Queue<(IExporterNode, ICollection<object>)> ExporterNodeQueue = new();
}