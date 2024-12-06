using WzComparerR2.Common;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class GearReadOptions : IReadOptions
{
    public GlobalStringData? GlobalStringData { get; set; }
    public string? GearDataJsonPath { get; set; }
    public string? GearIconOriginJsonPath { get; set; }
    public string? GearIconRawOriginJsonPath { get; set; }
    public string? GearIconPath { get; set; }
    public string? GearIconRawPath { get; set; }
}

public class GearReader(
    GlobalFindNodeFunction findNode,
    GearNodeRepository gearNodeRepository,
    ItemOptionNodeRepository itemOptionNodeRepository,
    GlobalStringDataProvider globalStringDataProvider)
    : AbstractWzReader<GearReadOptions>
{
    protected override INodeRepository GetNodeRepository(GearReadOptions _) => gearNodeRepository;

    protected override IList<INodeConverter<object>> GetConverters(GearReadOptions options)
    {
        var itemOptionData = new ItemOptionConverter(string.Empty).Convert(itemOptionNodeRepository.GetNodes());
        var converters = new List<INodeConverter<object>>();
        if (options.GearDataJsonPath != null)
            converters.Add(new GearConverter(options.GearDataJsonPath, globalStringDataProvider.GlobalStringData,
                itemOptionData, findNode));
        if (options.GearIconOriginJsonPath != null)
            converters.Add(new IconOriginConverter(options.GearIconOriginJsonPath, @"info\icon\origin"));
        if (options.GearIconRawOriginJsonPath != null)
            converters.Add(new IconOriginConverter(options.GearIconRawOriginJsonPath, @"info\iconRaw\origin"));
        if (options.GearIconPath != null)
            converters.Add(new IconBitmapConverter(options.GearIconPath, @"info\icon", findNode));
        if (options.GearIconRawPath != null)
            converters.Add(new IconBitmapConverter(options.GearIconRawPath, @"info\iconRaw", findNode));

        return converters;
    }
}