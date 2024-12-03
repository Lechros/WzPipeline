using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Parser;

public class GearParser(
    GlobalFindNodeFunction findNode,
    GearNodeRepository gearNodeRepository,
    StringEqpNodeRepository stringEqpNodeRepository,
    ItemOptionNodeRepository itemOptionNodeRepository)
    : AbstractWzParser
{
    public const string GearDataJsonPath = "gear-data.json";
    public const string GearIconOriginJsonPath = "gear-origin.json";
    public const string GearIconRawOriginJsonPath = "gear-origin-raw.json";
    public const string GearIconPath = "gear-icon";
    public const string GearIconRawPath = "gear-icon-raw";

    public bool ParseGearData { get; set; }
    public bool ParseGearIconOrigin { get; set; }
    public bool ParseGearIconRawOrigin { get; set; }
    public bool ParseGearIcon { get; set; }
    public bool ParseGearIconRaw { get; set; }

    protected override IEnumerable<Wz_Node> GetNodes() => gearNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        var gearStringData = new WzStringConverter().Convert(stringEqpNodeRepository.GetNodes());
        var itemOptionData = new ItemOptionConverter(string.Empty).Convert(itemOptionNodeRepository.GetNodes());
        var converters = new List<INodeConverter<object>>();
        if (ParseGearData)
            converters.Add(new GearConverter(GearDataJsonPath, gearStringData, itemOptionData, findNode));
        if (ParseGearIconOrigin)
            converters.Add(new IconOriginConverter(GearIconOriginJsonPath, @"info\icon\origin"));
        if (ParseGearIconRawOrigin)
            converters.Add(new IconOriginConverter(GearIconRawOriginJsonPath, @"info\iconRaw\origin"));
        if (ParseGearIcon)
            converters.Add(new IconBitmapConverter(GearIconPath, @"info\icon", findNode));
        if (ParseGearIconRaw)
            converters.Add(new IconBitmapConverter(GearIconRawPath, @"info\iconRaw", findNode));

        return converters;
    }
}