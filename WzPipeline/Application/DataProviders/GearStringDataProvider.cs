using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class GearStringDataProvider(GearStringBlockFactory factory)
    : AsyncDataProvider<IReadOnlyDictionary<string, GearStrings>>
{
    protected override async Task<IReadOnlyDictionary<string, GearStrings>> CreateAsync()
    {
        var data = new Dictionary<string, GearStrings>();

        var source = factory.CreateSource();
        var converter = factory.CreateConverter();
        var sink = factory.CreateDictionaryCollector(data);

        source.LinkTo(converter, new DataflowLinkOptions { PropagateCompletion = true });
        converter.LinkTo(sink, new DataflowLinkOptions { PropagateCompletion = true });

        await sink.Completion;

        return data;
    }
}