using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class ItemOptionReadOptions : IReadOptions
{
    public string? ItemOptionJsonPath { get; set; }
}

public class ItemOptionReader(ItemOptionNodeRepository itemOptionNodeRepository) : AbstractWzReader<ItemOptionReadOptions>
{
    protected override INodeRepository GetNodeRepository(ItemOptionReadOptions _) => itemOptionNodeRepository;

    protected override IList<INodeConverter<object>> GetConverters(ItemOptionReadOptions options)
    {
        var converters = new List<INodeConverter<object>>();
        if (options.ItemOptionJsonPath != null)
            converters.Add(new ItemOptionConverter(options.ItemOptionJsonPath));
        return converters;
    }
}