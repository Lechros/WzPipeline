namespace WzJson.Common.V2.Dsl;

public class ConverterBuilder<TNode, TResult> : IConverterBuilder<TNode>,
    IProcessorChainable<ConverterBuilder<TNode, TResult>, TResult>,
    IExporterChainable<ConverterBuilder<TNode, TResult>, TResult> where TNode : INode
{
    private readonly IConverter<TNode, TResult> _converter;
    private readonly List<IProcessorBuilder<TResult>> _chainedProcessors;
    private readonly List<IExporterBuilder<TResult>> _chainedExporters;

    internal ConverterBuilder(IConverter<TNode, TResult> converter)
    {
        _converter = converter;
        _chainedProcessors = [];
        _chainedExporters = [];
    }

    public IConverter<TNode, object> Get() => (IConverter<TNode, object>)_converter;
    public IEnumerable<IProcessorBuilder<TResult>> ChainedProcessors => _chainedProcessors;
    public IEnumerable<IExporterBuilder<TResult>> ChainedExporters => _chainedExporters;

    public ConverterBuilder<TNode, TResult> Processor<TNextOut>(IProcessor<TResult, TNextOut> next,
        Action<ProcessorBuilder<TResult, TNextOut>> config)
    {
        var processorBuilder = new ProcessorBuilder<TResult, TNextOut>(next);
        config(processorBuilder);
        _chainedProcessors.Add(processorBuilder);
        return this;
    }

    public ConverterBuilder<TNode, TResult> Processor<TNextOut>(Condition when, IProcessor<TResult, TNextOut> next,
        Action<ProcessorBuilder<TResult, TNextOut>> config)
    {
        if (when.Value) Processor(next, config);
        return this;
    }

    public ConverterBuilder<TNode, TResult> Exporter(IExporter<TResult> next, string path)
    {
        var exporterBuilder = new ExporterBuilder<TResult>(next, path);
        _chainedExporters.Add(exporterBuilder);
        return this;
    }

    public ConverterBuilder<TNode, TResult> Exporter(Condition when, IExporter<TResult> next, string path)
    {
        if (when.Value) Exporter(next, path);
        return this;
    }
}