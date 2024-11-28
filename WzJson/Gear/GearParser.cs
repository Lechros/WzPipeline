using WzComparerR2.WzLib;

namespace WzJson.Gear;

public class GearParser : IWzParser
{
    public const string GearDataJsonPath = "gear-data.json";
    public const string GearIconOriginJsonPath = "gear-origin.json";
    public const string GearIconRawOriginJsonPath = "gear-origin-raw.json";
    public const string GearIconPath = "gear-icon";
    public const string GearIconRawPath = "gear-icon-raw";

    private readonly IWzProvider wzProvider;
    private readonly GearNodeRepository gearNodeRepository;
    private readonly StringEqpNodeRepository stringEqpNodeRepository;

    public GearParser(IWzProvider wzProvider, GearNodeRepository gearNodeRepository,
        StringEqpNodeRepository stringEqpNodeRepository)
    {
        this.wzProvider = wzProvider;
        this.gearNodeRepository = gearNodeRepository;
        this.stringEqpNodeRepository = stringEqpNodeRepository;
    }

    public bool ParseGearData { get; set; }
    public bool ParseGearIconOrigin { get; set; }
    public bool ParseGearIconRawOrigin { get; set; }
    public bool ParseGearIcon { get; set; }
    public bool ParseGearIconRaw { get; set; }

    public IList<IData> Parse()
    {
        var converters = GetConverters();
        var datas = new List<IData>();

        foreach (var converter in converters)
        {
            var data = converter.Convert(gearNodeRepository.GetNodes());
            datas.Add(data);
        }

        return datas;
    }

    private IList<IDataConverter<IData>> GetConverters()
    {
        var nameDescData = new NameDescConverter().Convert(stringEqpNodeRepository.GetNodes());
        var converters = new List<IDataConverter<IData>>();
        if (ParseGearData)
            converters.Add(new GearConverter(GearDataJsonPath, nameDescData, wzProvider.FindNodeFunction));
        if (ParseGearIconOrigin)
            converters.Add(new IconOriginConverter(GearIconOriginJsonPath, @"info\icon\origin"));
        if (ParseGearIconRawOrigin)
            converters.Add(new IconOriginConverter(GearIconRawOriginJsonPath, @"info\iconRaw\origin"));
        if (ParseGearIcon)
            converters.Add(new IconBitmapConverter(GearIconPath, @"info\icon", wzProvider.FindNodeFunction));
        if (ParseGearIconRaw)
            converters.Add(new IconBitmapConverter(GearIconRawPath, @"info\iconRaw", wzProvider.FindNodeFunction));

        return converters;
    }
}