namespace WzJson.V2.Core.Pipeline.Runner;

public class PipelineRunner
{
    public static void Run(RootNode root)
    {
        var context = new ExecutionContext();
        foreach (var (node, _) in DfsNodes(root, 0))
        {
            switch (node.Type)
            {
                case PipelineNodeType.Traverser:
                    context.TraverserNodes.Add((ITraverserNode)node);
                    break;
            }
        }

        foreach (var traverserNode in context.TraverserNodes)
        {
            IReadOnlyList<(IConverterNode, List<object>)> converters = traverserNode.Children
                .Select(converterNode => ((IConverterNode)converterNode, new List<object>())).ToList();
            foreach (var node in traverserNode.Traverser.EnumerateNodes())
            {
                foreach (var (converterNode, converterResults) in converters)
                {
                    var convertResult = converterNode.Converter.Convert(node);
                    if (convertResult != null)
                    {
                        converterResults.Add(convertResult);
                    }
                }
            }

            foreach (var (converterNode, converterResults) in converters)
            {
                foreach (var child in converterNode.Children)
                {
                    switch (child.Type)
                    {
                        case PipelineNodeType.Processor:
                            context.ProcessorNodeQueue.Enqueue(((IProcessorNode)child, converterResults));
                            break;
                        case PipelineNodeType.Exporter:
                            context.ExporterNodeQueue.Enqueue(((IExporterNode)child, converterResults));
                            break;
                        default:
                            throw new InvalidOperationException(
                                $"Invalid child node type for ConverterNode: {child.Type}");
                    }
                }
            }
        }

        while (context.ProcessorNodeQueue.Count > 0)
        {
            var (processorNode, input) = context.ProcessorNodeQueue.Dequeue();
            var processResult = EnsureCollection(processorNode.Processor.Process(input));
            foreach (var child in processorNode.Children)
            {
                switch (child.Type)
                {
                    case PipelineNodeType.Processor:
                        context.ProcessorNodeQueue.Enqueue(((IProcessorNode)child, processResult));
                        break;
                    case PipelineNodeType.Exporter:
                        context.ExporterNodeQueue.Enqueue(((IExporterNode)child, processResult));
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid child node type for ConverterNode: {child.Type}");
                }
            }
        }

        var exportTasks = new List<Task>(context.ExporterNodeQueue.Count);
        while (context.ExporterNodeQueue.Count > 0)
        {
            var (exporterNode, inputs) = context.ExporterNodeQueue.Dequeue();
            var task = Parallel.ForEachAsync(inputs,
                async (input, _) =>
                {
                    await exporterNode.Exporter.Export(input);
                });
            exportTasks.Add(task);
        }

        Task.WhenAll(exportTasks).Wait();
    }

    private static IEnumerable<(IPipelineNode, int)> DfsNodes(IPipelineNode node, int depth)
    {
        yield return (node, depth);
        foreach (var child in node.Children)
        {
            foreach (var descendantDepth in DfsNodes(child, depth + 1))
            {
                yield return descendantDepth;
            }
        }
    }

    private static ICollection<T> EnsureCollection<T>(IEnumerable<T> enumerable)
    {
        return enumerable switch
        {
            ICollection<T> generic => generic,
            _ => enumerable.ToList()
        };
    }
}