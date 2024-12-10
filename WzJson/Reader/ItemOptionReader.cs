using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Converter;
using WzJson.Model;
using WzJson.Repository;

namespace WzJson.Reader;

public class ItemOptionReadOptions : IReadOptions
{
    public string? ItemOptionJsonPath { get; set; }
}

public class ItemOptionReader(ItemOptionNodeRepository itemOptionNodeRepository)
    : AbstractWzReader<ItemOptionReadOptions>
{
    protected override INodeRepository GetNodeRepository(ItemOptionReadOptions _) => itemOptionNodeRepository;

    protected override IList<INodeProcessor> GetProcessors(ItemOptionReadOptions options)
    {
        var processors = new List<INodeProcessor>();
        if (options.ItemOptionJsonPath != null)
            processors.Add(DefaultNodeProcessor.Of(new ItemOptionConverter(),
                () => new JsonData<ItemOption>("item option data", options.ItemOptionJsonPath)));
        return processors;
    }
}