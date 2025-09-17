namespace WzJson.V2.Pipeline.Graph;

internal static class GraphSystemRunner
{
    public static void Run(GraphSystem.RootNode root)
    {
        foreach (IRepositoryNode repositoryNode in root.Children)
        {
            IReadOnlyList<(IConverterNode, List<object>)> converterPairs = repositoryNode.Children
                .Select(converterNode => ((IConverterNode)converterNode, new List<object>())).ToList();
            foreach (var node in repositoryNode.Repository.EnumerateNodes())
            {
                foreach (var (converterNode, convertResults) in converterPairs)
                {
                    var convertResult = converterNode.Converter.Convert(node);
                    if (convertResult != null)
                    {
                        convertResults.Add(convertResult);
                    }
                }
            }

            foreach (var (converterNode, convertResults) in converterPairs)
            {
                foreach (var childNode in converterNode.Children)
                {
                    switch (childNode)
                    {
                        case IProcessorNode processorNode: // recursive call
                            ExecuteProcessor(processorNode, convertResults);
                            break;
                        case IExporterNode exporterNode:
                            ExecuteExporter(exporterNode, convertResults);
                            break;
                    }
                }
            }
        }
    }

    private static void ExecuteProcessor(IProcessorNode processorNode, IEnumerable<object> inputs)
    {
        var processor = ((ProcessorNode)processorNode).Processor;
        var outputs = EnsureCollection(processor.Process(inputs));
        foreach (var childNode in processorNode.Children)
        {
            switch (childNode)
            {
                case IProcessorNode childProcessorNode:
                    ExecuteProcessor(childProcessorNode, outputs);
                    break;
                case IExporterNode childExporterNode:
                    ExecuteExporter(childExporterNode, outputs);
                    break;
            }
        }
    }

    private static void ExecuteExporter(IExporterNode exporterNode, IEnumerable<object> inputs)
    {
        exporterNode.Exporter.Export(inputs, exporterNode.Path);
    }

    private static ICollection<T> EnsureCollection<T>(IEnumerable<T> enumerable)
    {
        if (enumerable is ICollection<T> collection)
        {
            return collection;
        }

        return enumerable.ToList();
    }
}