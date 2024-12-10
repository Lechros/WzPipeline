using WzJson.Common;
using WzJson.Common.Data;
using WzJson.Converter;
using WzJson.Model;
using WzJson.Repository;

namespace WzJson.Reader;

public class SetItemReadOptions : IReadOptions
{
    public string? SetItemJsonName { get; set; }
}

public class SetItemReader(
    SetItemNodeRepository setItemNodeRepository,
    SetItemConverter setItemConverter) : AbstractWzReader<SetItemReadOptions>
{
    protected override INodeRepository GetNodeRepository(SetItemReadOptions _) => setItemNodeRepository;

    protected override IList<INodeProcessor> GetProcessors(SetItemReadOptions options)
    {
        var processors = new List<INodeProcessor>();
        if (options.SetItemJsonName != null)
            processors.Add(DefaultNodeProcessor.Of(setItemConverter,
                () => new JsonData<SetItem>("set item data", options.SetItemJsonName)));
        return processors;
    }
}