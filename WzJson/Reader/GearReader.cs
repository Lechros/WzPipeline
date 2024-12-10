using WzComparerR2.Common;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Common.Data;
using WzJson.Converter;
using WzJson.DataProvider;
using WzJson.Model;
using WzJson.Repository;

namespace WzJson.Reader;

public class GearReadOptions : IReadOptions
{
    public string? GearDataJsonPath { get; set; }
    public string? GearIconOriginJsonPath { get; set; }
    public string? GearIconRawOriginJsonPath { get; set; }
    public string? GearIconPath { get; set; }
    public string? GearIconRawPath { get; set; }
}

public class GearReader(
    GlobalFindNodeFunction findNode,
    GearNodeRepository gearNodeRepository,
    GearConverter gearConverter)
    : AbstractWzReader<GearReadOptions>
{
    protected override INodeRepository GetNodeRepository(GearReadOptions _) => gearNodeRepository;

    protected override IList<INodeProcessor> GetProcessors(GearReadOptions options)
    {
        var processors = new List<INodeProcessor>();
        if (options.GearDataJsonPath != null)
            processors.Add(DefaultNodeProcessor.Of(gearConverter,
                () => new JsonData<Gear>("gear data", options.GearDataJsonPath)));
        if (options.GearIconOriginJsonPath != null)
            processors.Add(DefaultNodeProcessor.Of(new IconOriginConverter(@"info\icon\origin"),
                () => new JsonData<int[]>("gear icon origins", options.GearIconOriginJsonPath)));
        if (options.GearIconRawOriginJsonPath != null)
            processors.Add(DefaultNodeProcessor.Of(new IconOriginConverter(@"info\iconRaw\origin"),
                () => new JsonData<int[]>("gear raw icon origins", options.GearIconRawOriginJsonPath)));
        if (options.GearIconPath != null)
            processors.Add(DefaultNodeProcessor.Of(new IconBitmapConverter(@"info\icon", findNode),
                () => new BitmapData("gear icons", options.GearIconPath)));
        if (options.GearIconRawPath != null)
            processors.Add(DefaultNodeProcessor.Of(new IconBitmapConverter(@"info\iconRaw", findNode),
                () => new BitmapData("gear raw icons", options.GearIconRawPath)));
        return processors;
    }
}