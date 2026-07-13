using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class AstraSubWeaponDataProvider(AstraSubWeaponBlockFactory factory) : AsyncDataProvider<AstraSubWeaponData>
{
    protected override async Task<AstraSubWeaponData> CreateAsync()
    {
        var data = new AstraSubWeaponData();

        var source = factory.CreateSource();
        var converter = factory.CreateConverter();
        var sink = factory.CreateDictionaryCollector(data);

        source.LinkTo(converter, new DataflowLinkOptions { PropagateCompletion = true });
        converter.LinkTo(sink, new DataflowLinkOptions { PropagateCompletion = true });

        await sink.Completion;

        return data;
    }
}