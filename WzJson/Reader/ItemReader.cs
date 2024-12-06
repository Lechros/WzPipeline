using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class ItemReadOptions : IReadOptions
{
    public string? ItemIconOriginJsonPath { get; set; }
    public string? ItemIconPath { get; set; }
}

public class ItemReader(ItemNodeRepository itemNodeRepository, GlobalFindNodeFunction findNode)
    : AbstractWzReader<ItemReadOptions>
{
    protected override INodeRepository GetNodeRepository(ItemReadOptions _) => itemNodeRepository;

    protected override IList<INodeConverter<object>> GetConverters(ItemReadOptions options)
    {
        var converters = new List<INodeConverter<object>>();
        if (options.ItemIconOriginJsonPath != null)
            converters.Add(new IconOriginConverter("item icon origins", options.ItemIconOriginJsonPath, @"info\icon\origin"));
        if (options.ItemIconPath != null)
            converters.Add(new IconBitmapConverter("item icons", options.ItemIconPath, @"info\icon", findNode));
        return converters;
    }
}