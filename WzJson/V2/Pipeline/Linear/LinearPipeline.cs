using WzJson.V2.Pipeline.Graph;

namespace WzJson.V2.Pipeline.Linear;

public class LinearPipeline<T>(RootNode root)
{
    public T Run()
    {
        var traverserNode = (ITraverserNode)root.Children.Single();
        var converterNode = (IConverterNode)traverserNode.Children.Single();
        var convertResults = new List<object>();
        foreach (var node in traverserNode.Traverser.EnumerateNodes())
        {
            var convertResult = converterNode.Converter.Convert(node);
            if (convertResult != null)
            {
                convertResults.Add(convertResult);
            }
        }

        IEnumerable<object> results = convertResults;
        IGraphNode curNode = converterNode;
        while (curNode.Children.Count > 0)
        {
            var childNode = (IProcessorNode)curNode.Children.Single();
            results = childNode.Processor.Process(results);
            curNode = childNode;
        }

        return (T)results.Single();
    }
}