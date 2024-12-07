using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class SetItemReadOptions : IReadOptions
{
    public string? SetItemJsonName { get; set; }
}

public class SetItemReader(
    SetItemNodeRepository setItemNodeRepository,
    ItemOptionNodeRepository itemOptionNodeRepository) : AbstractWzReader<SetItemReadOptions>
{
    protected override INodeRepository GetNodeRepository(SetItemReadOptions _) => setItemNodeRepository;

    protected override IList<INodeConverter<object>> GetConverters(SetItemReadOptions options)
    {
        var itemOptionData = new ItemOptionConverter(string.Empty, string.Empty).Convert(itemOptionNodeRepository.GetNodes());
        var converters = new List<INodeConverter<object>>();
        if (options.SetItemJsonName != null)
            converters.Add(new SetItemConverter("set item data", options.SetItemJsonName, itemOptionData));
        return converters;
    }
}