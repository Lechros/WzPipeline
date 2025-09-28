using System.Net.Mime;

namespace WzJson.V2.Core.Pipeline.Runner;

internal static class PipelineRunner
{
    public static ExecutionContext Run(RootNode root, IProgress<INodeState>? progress = null)
    {
        var ctx = new ExecutionContext(root, progress);
        ctx.StartWithTotalCount(ctx.TraverserNodes.Count, root);

        var processorQueue = new Queue<(IProcessorNode, ICollection<object>)>();
        var exporterQueue = new Queue<(IExporterNode, ICollection<object>)>();

        foreach (var traverserNode in ctx.TraverserNodes)
        {
            var converterNodes = traverserNode.Children.Select(node => (IConverterNode)node).ToArray();
            var converterPairs = converterNodes.Select(converter => (converter, new List<object>())).ToArray();

            var totalNodeCount = traverserNode.Traverser.GetNodeCount();
            ctx.StartWithTotalCount(totalNodeCount, [traverserNode, ..converterNodes]).Report();

            foreach (var node in traverserNode.Traverser.EnumerateNodes())
            {
                foreach (var (converterNode, converterResults) in converterPairs)
                {
                    var convertResult = converterNode.Converter.Convert(node);
                    if (convertResult != null)
                    {
                        converterResults.Add(convertResult);
                    }
                }

                ctx.IncrementCount([traverserNode, ..converterNodes]).Report();
            }

            ctx.Complete([traverserNode, ..converterNodes]).Report();

            foreach (var (converterNode, converterResults) in converterPairs)
            {
                foreach (var child in converterNode.Children)
                {
                    switch (child.Type)
                    {
                        case PipelineNodeType.Processor:
                            processorQueue.Enqueue(((IProcessorNode)child, converterResults));
                            break;
                        case PipelineNodeType.Exporter:
                            exporterQueue.Enqueue(((IExporterNode)child, converterResults));
                            break;
                        default:
                            throw new InvalidOperationException(
                                $"Invalid child node type for ConverterNode: {child.Type}");
                    }

                    ctx.SetTotalCountBeforeStart(converterResults.Count, child);
                }
            }

            ctx.IncrementCount(root).Report();
        }

        while (processorQueue.Count > 0)
        {
            var (processorNode, input) = processorQueue.Dequeue();
            ctx.StartWithTotalCount(input.Count, processorNode).Report();
            var processResult = EnsureCollection(processorNode.Processor.Process(input));
            ctx.CompleteWithCount(input.Count, processorNode);

            foreach (var child in processorNode.Children)
            {
                switch (child.Type)
                {
                    case PipelineNodeType.Processor:
                        processorQueue.Enqueue(((IProcessorNode)child, processResult));
                        break;
                    case PipelineNodeType.Exporter:
                        exporterQueue.Enqueue(((IExporterNode)child, processResult));
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid child node type for ConverterNode: {child.Type}");
                }

                ctx.SetTotalCountBeforeStart(processResult.Count, child);
            }

            ctx.Report();
        }

        var exportTasks = new List<Task>(exporterQueue.Count);
        while (exporterQueue.Count > 0)
        {
            var (exporterNode, inputs) = exporterQueue.Dequeue();
            var state = ctx.GetNodeState(exporterNode);
            state.Start();
            ctx.Report();

            var task = Task.Run(async () =>
            {
                await Parallel.ForEachAsync(inputs, async (input, _) =>
                {
                    await exporterNode.Exporter.Export(input);
                    state.IncrementCount();
                    ctx.Report();
                });

                state.Complete();
                ctx.Report();
            });

            exportTasks.Add(task);
        }

        Task.WhenAll(exportTasks).Wait();

        ctx.Complete(root).Report();

        return ctx;
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