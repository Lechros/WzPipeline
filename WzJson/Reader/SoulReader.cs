using WzJson.Common;
using WzJson.Converter;
using WzJson.Repository;

namespace WzJson.Reader;

public class SoulReadOptions : IReadOptions
{
    public string? SoulDataJsonPath { get; set; }
}

public class SoulReader(
    SoulNodeRepository soulNodeRepository,
    GlobalStringDataProvider globalStringDataProvider)
    : AbstractWzReader<SoulReadOptions>
{
    protected override INodeRepository GetNodeRepository(SoulReadOptions _) => soulNodeRepository;

    protected override IList<INodeConverter<object>> GetConverters(SoulReadOptions options)
    {
        var converters = new List<INodeConverter<object>>();
        if (options.SoulDataJsonPath != null)
            converters.Add(new SoulConverter("soul data", options.SoulDataJsonPath, globalStringDataProvider.GlobalStringData, null));
        return converters;
    }
}