using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Common.Converter;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class GearReader(
    GlobalFindNodeFunction findNode,
    GearNodeRepository gearNodeRepository,
    ItemOptionNodeRepository itemOptionNodeRepository,
    GlobalStringData globalStringData)
    : AbstractWzReader
{
    public const string GearDataJsonPath = "gear-data.json";
    public const string GearIconOriginJsonPath = "gear-origin.json";
    public const string GearIconRawOriginJsonPath = "gear-origin-raw.json";
    public const string GearIconPath = "gear-icon";
    public const string GearIconRawPath = "gear-icon-raw";

    public bool ReadGearData { get; set; }
    public bool ReadGearIconOrigin { get; set; }
    public bool ReadGearIconRawOrigin { get; set; }
    public bool ReadGearIcon { get; set; }
    public bool ReadGearIconRaw { get; set; }

    protected override IEnumerable<Wz_Node> GetNodes() => gearNodeRepository.GetNodes();

    protected override IList<INodeConverter<object>> GetConverters()
    {
        var itemOptionData = new ItemOptionConverter(string.Empty).Convert(itemOptionNodeRepository.GetNodes());
        var converters = new List<INodeConverter<object>>();
        if (ReadGearData)
            converters.Add(new GearConverter(GearDataJsonPath, globalStringData, itemOptionData, findNode));
        if (ReadGearIconOrigin)
            converters.Add(new IconOriginConverter(GearIconOriginJsonPath, @"info\icon\origin"));
        if (ReadGearIconRawOrigin)
            converters.Add(new IconOriginConverter(GearIconRawOriginJsonPath, @"info\iconRaw\origin"));
        if (ReadGearIcon)
            converters.Add(new IconBitmapConverter(GearIconPath, @"info\icon", findNode));
        if (ReadGearIconRaw)
            converters.Add(new IconBitmapConverter(GearIconRawPath, @"info\iconRaw", findNode));

        return converters;
    }
}