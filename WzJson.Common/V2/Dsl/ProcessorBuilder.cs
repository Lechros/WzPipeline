namespace WzJson.Common.V2.Dsl;

public class ProcessorBuilder<TIn, TOut> : IProcessorBuilder<TIn>,
    IProcessorChainable<ProcessorBuilder<TIn, TOut>, TOut>,
    IExporterChainable<ProcessorBuilder<TIn, TOut>, TOut>
{
    private readonly IProcessor<TIn, TOut> _processor;
    private readonly List<IProcessorBuilder<TOut>> _chainedProcessors;
    private readonly List<IExporterBuilder<TOut>> _chainedExporters;

    internal ProcessorBuilder(IProcessor<TIn, TOut> processor)
    {
        _processor = processor;
        _chainedProcessors = [];
        _chainedExporters = [];
    }

    public IProcessor<TIn, object> Get() => (IProcessor<TIn, object>)_processor;
    public IEnumerable<IProcessorBuilder<TOut>> ChainedProcessors => _chainedProcessors;
    public IEnumerable<IExporterBuilder<TOut>> ChainedExporters => _chainedExporters;

    public ProcessorBuilder<TIn, TOut> Processor<TNextOut>(IProcessor<TOut, TNextOut> next,
        Action<ProcessorBuilder<TOut, TNextOut>> config)
    {
        var processorBuilder = new ProcessorBuilder<TOut, TNextOut>(next);
        config(processorBuilder);
        _chainedProcessors.Add(processorBuilder);
        return this;
    }

    public ProcessorBuilder<TIn, TOut> Processor<TNextOut>(Condition when, IProcessor<TOut, TNextOut> next,
        Action<ProcessorBuilder<TOut, TNextOut>> config)
    {
        if (when.Value) Processor(next, config);
        return this;
    }

    public ProcessorBuilder<TIn, TOut> Exporter(IExporter<TOut> next, string path)
    {
        var exporterBuilder = new ExporterBuilder<TOut>(next, path);
        _chainedExporters.Add(exporterBuilder);
        return this;
    }

    public ProcessorBuilder<TIn, TOut> Exporter(Condition when, IExporter<TOut> next, string path)
    {
        if (when.Value) Exporter(next, path);
        return this;
    }
}