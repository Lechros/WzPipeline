using System.Drawing;
using WzComparerR2.Common;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Common.Data;
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

    protected override IList<INodeProcessor> GetProcessors(ItemReadOptions options)
    {
        var processors = new List<INodeProcessor>();
        if (options.ItemIconOriginJsonPath != null)
            processors.Add(DefaultNodeProcessor.Of(new IconOriginConverter(@"info\icon\origin"),
                () => new JsonData<int[]>("item icon origins", options.ItemIconOriginJsonPath)));
        if (options.ItemIconPath != null)
            processors.Add(DefaultNodeProcessor.Of(new IconBitmapConverter(@"info\icon", findNode),
                () => new JsonData<Bitmap>("item icons", options.ItemIconPath)));
        return processors;
    }
}