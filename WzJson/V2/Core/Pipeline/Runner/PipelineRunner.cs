namespace WzJson.V2.Core.Pipeline.Runner;

internal static class PipelineRunner
{
    public static ExecutionContext Run(PipelineRoot root, IProgress<IStepState>? progress = null)
    {
        var ctx = new ExecutionContext(root, progress);
        ctx.StartWithTotalCount(ctx.TraverserSteps.Count, root);

        var processorQueue = new Queue<(IProcessorStep, ICollection<object>)>();
        var exporterList = new List<(IExporterStep, ICollection<object>)>();

        foreach (var traverserNode in ctx.TraverserSteps)
        {
            var converterNodes = traverserNode.Children.Select(node => (IConverterStep)node).ToArray();
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
                        case StepType.Processor:
                            processorQueue.Enqueue(((IProcessorStep)child, converterResults));
                            break;
                        case StepType.Exporter:
                            exporterList.Add(((IExporterStep)child, converterResults));
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
                    case StepType.Processor:
                        processorQueue.Enqueue(((IProcessorStep)child, processResult));
                        break;
                    case StepType.Exporter:
                        exporterList.Add(((IExporterStep)child, processResult));
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid child node type for ConverterNode: {child.Type}");
                }

                ctx.SetTotalCountBeforeStart(processResult.Count, child);
            }

            ctx.Report();
        }

        var exportTasks = new List<Task>(exporterList.Count);
        foreach (var (exporterNode, inputs) in exporterList)
        {
            var state = ctx.GetStepState(exporterNode);
            state.Start();
            ctx.Report();

            var exportTask = Task.Run(async () =>
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

            exportTasks.Add(exportTask);
        }

        Task.WhenAll(exportTasks).Wait();

        Parallel.ForEach(exporterList, (tuple, _) =>
        {
            var (exporterStep, inputs) = tuple;
            Parallel.ForEach(inputs, input => { exporterStep.Exporter.Cleanup(input); });
        });
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