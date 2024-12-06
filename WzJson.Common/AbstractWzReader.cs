namespace WzJson.Common;

public abstract class AbstractWzReader<TReadOptions> : IWzReader where TReadOptions : IReadOptions
{
    public IList<IData> Read(IReadOptions options, IProgress<ReadProgressData> progress)
    {
        return Read((TReadOptions)options, progress);
    }

    public IList<IData> Read(TReadOptions options, IProgress<ReadProgressData> progress)
    {
        var repository = GetNodeRepository(options);
        var converters = GetConverters(options);
        var pairs = converters.Select(converter => new ConverterDataPair(converter)).ToArray();

        var total = repository.GetNodeCount();
        var current = 0;
        progress.Report(new ReadProgressData(current, total));
        
        foreach (var node in repository.GetNodes())
        {
            foreach (var pair in pairs)
            {
                var name = pair.Converter.GetNodeKey(node);
                var item = pair.Converter.ConvertNode(node, name);
                if (item != null)
                    pair.Data.Add(name, item);
            }

            current++;
            if (total < 100 || current % (total / 100) == 0)
                progress.Report(new ReadProgressData(current, total));
        }

        progress.Report(new ReadProgressData(total, total));

        return pairs.Select(pair => pair.Data).ToList();
    }

    protected abstract INodeRepository GetNodeRepository(TReadOptions options);

    protected abstract IList<INodeConverter<object>> GetConverters(TReadOptions options);

    private record struct ConverterDataPair(INodeConverter<object> Converter)
    {
        public readonly INodeConverter<object> Converter = Converter;
        public readonly IData Data = Converter.NewData();
    }
}