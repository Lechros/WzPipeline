namespace WzJson.Common;

public abstract class AbstractWzReader<TReadOptions> : IWzReader where TReadOptions : IReadOptions
{
    public IReadOnlyList<IKeyValueData> Read(TReadOptions options, IProgress<ReadProgressData> progress)
    {
        var repository = GetNodeRepository(options);
        var converters = GetConverters(options);
        var pairs = converters.Select(converter => new ConverterDataPair(converter)).ToArray();

        var reporter = new ProgressReporter<ReadProgressData>(progress,
            (current, total) => new ReadProgressData(current, total), repository.GetNodeCount());

        foreach (var node in repository.GetNodes())
        {
            foreach (var pair in pairs)
            {
                var name = pair.Converter.GetNodeKey(node);
                var item = pair.Converter.ConvertNode(node, name);
                if (item != null)
                    pair.Data.Add(name, item);
            }

            reporter.Increment();
        }

        reporter.Complete();

        return pairs.Select(pair => pair.Data).ToList();
    }

    IReadOnlyList<IData> IWzReader.Read(IReadOptions options, IProgress<ReadProgressData> progress)
    {
        return Read((TReadOptions)options, progress);
    }
    
    protected abstract INodeRepository GetNodeRepository(TReadOptions options);

    protected abstract IList<INodeConverter<object>> GetConverters(TReadOptions options);

    private record struct ConverterDataPair(INodeConverter<object> Converter)
    {
        public readonly INodeConverter<object> Converter = Converter;
        public readonly IKeyValueData Data = Converter.NewData();
    }
}