using WzJson.Core.Pipeline.Dsl;

namespace WzJson.Core.Pipeline;

public class GraphSystem(GraphSystem.RootNode root)
{
    public static RootConfig Builder()
    {
        RootNode root = new RootNode();
        return new RootConfig(root);
    }

    public void Prune()
    {
        PruneNode(root);
    }

    private void PruneNode(IGraphNode node)
    {
        var children = node.Children;
        for (int i = children.Count - 1; i >= 0; i--)
        {
            var child = children[i];

            if (child.Children.Count > 0)
            {
                PruneNode(child);
            }

            if (child is not IExporterNode && child.Children.Count == 0)
            {
                children.RemoveAt(i);
            }
        }
    }

    public void Run()
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

    private void ExecuteProcessor(IProcessorNode processorNode, IEnumerable<object> inputs)
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

    private void ExecuteExporter(IExporterNode exporterNode, IEnumerable<object> inputs)
    {
        exporterNode.Exporter.Export(inputs, exporterNode.Path);
    }

    private ICollection<T> EnsureCollection<T>(IEnumerable<T> enumerable)
    {
        if (enumerable is ICollection<T> collection)
        {
            return collection;
        }

        return enumerable.ToList();
    }

    public class RootNode : IGraphNode
    {
        public IGraphNode? Parent => null;
        public IList<IGraphNode> Children { get; } = [];
    }
}