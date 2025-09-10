namespace WzJson.Common.V2.Dsl;

public class RepositoryBuilder<TNode>
    : IRepositoryBuilder, IConverterChainable<RepositoryBuilder<TNode>, TNode> where TNode : INode
{
    private readonly INodeRepository2<TNode> _repository;
    private readonly List<IConverterBuilder<TNode>> _chainedConverters;

    internal RepositoryBuilder(INodeRepository2<TNode> repository)
    {
        _repository = repository;
        _chainedConverters = [];
    }

    public INodeRepository2<INode> Get() => (INodeRepository2<INode>)_repository;
    public IEnumerable<IConverterBuilder<TNode>> ChainedConverters => _chainedConverters;

    public RepositoryBuilder<TNode> Converter<TNextOut>(IConverter<TNode, TNextOut> next,
        Action<ConverterBuilder<TNode, TNextOut>> config)
    {
        var converterBuilder = new ConverterBuilder<TNode, TNextOut>(next);
        config(converterBuilder);
        _chainedConverters.Add(converterBuilder);
        return this;
    }

    public RepositoryBuilder<TNode> Converter<TNextOut>(Condition when, IConverter<TNode, TNextOut> next,
        Action<ConverterBuilder<TNode, TNextOut>> config)
    {
        if (when.Value) Converter(next, config);
        return this;
    }
}