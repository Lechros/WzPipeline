using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Converter;
using WzJson.DataProvider;
using WzJson.Model;
using WzJson.Repository;

namespace WzJson.Reader;

public class SoulReadOptions : IReadOptions
{
    public string? SoulDataJsonPath { get; set; }
}

public class SoulReader(
    SoulNodeRepository soulNodeRepository,
    SoulConverter soulConverter)
    : AbstractWzReader<SoulReadOptions>
{
    protected override INodeRepository GetNodeRepository(SoulReadOptions _) => soulNodeRepository;

    protected override IList<INodeProcessor> GetProcessors(SoulReadOptions options)
    {
        var processors = new List<INodeProcessor>();
        if (options.SoulDataJsonPath != null)
            processors.Add(DefaultNodeProcessor.Of(soulConverter,
                () => new JsonData<Soul>("soul data", options.SoulDataJsonPath)));
        return processors;
    }
}