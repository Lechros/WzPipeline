using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.SetItem;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

class SetItemDataProvider(SetItemDataBlockFactory factory, ItemOptionDataProvider itemOptionDataProvider)
    : AsyncDataProvider<SortedDictionary<int, MalibSetItem>>
{
    protected override async Task<SortedDictionary<int, MalibSetItem>> CreateAsync()
    {
        var itemOptionData = await itemOptionDataProvider.GetAsync();
        var data = new SortedDictionary<int, MalibSetItem>();

        var source = factory.CreateSource();
        var parser = factory.CreateParser(itemOptionData);
        var sink = factory.CreateDictionaryCollector(data);

        source.LinkTo(parser, new DataflowLinkOptions { PropagateCompletion = true });
        parser.LinkTo(sink, new DataflowLinkOptions { PropagateCompletion = true });

        await sink.Completion;

        return data;
    }
}