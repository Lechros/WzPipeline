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
        var datas = converters.Select(converter => converter.NewData()).ToList();

        foreach (var node in gearNodeRepository.GetNodes())
        {
            for (var i = 0; i < converters.Count; i++)
            {
                var converter = converters[i];
                var data = datas[i];
                var name = converter.GetNodeName(node);
                var item = converter.ConvertNode(node, name);
                if (item != null)
                    data.Add(name, item);
            }
        }

        return datas;
    }

    private IList<INodeConverter<object>> GetConverters()
    {
        var nameDescData = new NameDescConverter().Convert(stringEqpNodeRepository.GetNodes());
        var converters = new List<INodeConverter<object>>();
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