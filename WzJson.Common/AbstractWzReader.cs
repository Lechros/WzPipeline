namespace WzJson.Common;

public abstract class AbstractWzReader<TReadOptions> : IWzReader where TReadOptions : IReadOptions
{
    public IReadOnlyList<IKeyValueData> Read(TReadOptions options, IProgress<ReadProgressData> progress)
    {
        var repository = GetNodeRepository(options);
        var processors = GetProcessors(options);
        var units = processors.Select(processor => new ProcessUnit(processor.Converter, processor.CreateData()))
            .ToArray();

        var reporter = new ProgressReporter<ReadProgressData>(progress,
            (current, total) => new ReadProgressData(current, total), repository.GetNodeCount());

        foreach (var node in repository.GetNodes())
        {
            foreach (var unit in units)
            {
                unit.ConvertNodeAndAdd(node);
            }

            reporter.Increment();
        }

        reporter.Complete();

        return units.Select(unit => unit.Data).ToList();
    }

    IReadOnlyList<IData> IWzReader.Read(IReadOptions options, IProgress<ReadProgressData> progress)
    {
        return Read((TReadOptions)options, progress);
    }

    protected abstract INodeRepository GetNodeRepository(TReadOptions options);

    protected abstract IList<INodeProcessor> GetProcessors(TReadOptions options);
}