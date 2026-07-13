using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class ItemOptionDataProvider(ItemOptionBlockFactory factory) : AsyncDataProvider<ItemOptionData>
{
    protected override async Task<ItemOptionData> CreateAsync()
    {
        var data = new ItemOptionData();

        var source = factory.CreateSource();
        var parser = factory.CreateParser();
        var sink = factory.CreateDictionaryCollector(data);

        source.LinkTo(parser, new DataflowLinkOptions { PropagateCompletion = true });
        parser.LinkTo(sink, new DataflowLinkOptions { PropagateCompletion = true });

        await sink.Completion;

        return data;
    }
}