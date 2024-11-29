using WzComparerR2.WzLib;

namespace WzJson.Common;

public abstract class AbstractWzParser : IWzParser
{
    public IList<IData> Parse()
    {
        var converters = GetConverters();
        var pairs = converters.Select(converter => new ConverterDataPair(converter)).ToArray();

        foreach (var node in GetNodes())
        {
            foreach (var pair in pairs)
            {
                var name = pair.Converter.GetNodeName(node);
                var item = pair.Converter.ConvertNode(node, name);
                if (item != null)
                    pair.Data.Add(name, item);
            }
        }

        return pairs.Select(pair => pair.Data).ToList();
    }

    protected abstract IEnumerable<Wz_Node> GetNodes();

    protected abstract IList<INodeConverter<object>> GetConverters();

    private record struct ConverterDataPair(INodeConverter<object> Converter)
    {
        public readonly INodeConverter<object> Converter = Converter;
        public readonly IData Data = Converter.NewData();
    }
}