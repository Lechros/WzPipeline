using System.Threading.Tasks.Dataflow;
using WzPipeline.Domains.Gear;
using WzPipeline.Shared;

namespace WzPipeline.Application.DataProviders;

public class GearDataProvider(
    GearDataBlockFactory factory,
    GearStringDataProvider gearStringDataProvider,
    ItemOptionDataProvider itemOptionDataProvider,
    AstraSubWeaponDataProvider astraSubWeaponDataProvider,
    SkillNameDataProvider skillNameDataProvider) : AsyncDataProvider<SortedDictionary<int, MalibGear>>
{
    protected override async Task<SortedDictionary<int, MalibGear>> CreateAsync()
    {
        var gearStringTask = gearStringDataProvider.GetAsync();
        var itemOptionTask = itemOptionDataProvider.GetAsync();
        var astraSubWeaponTask = astraSubWeaponDataProvider.GetAsync();
        var skillNameTask = skillNameDataProvider.GetAsync();

        // await Task.WhenAll(
        //     gearStringTask,
        //     itemOptionTask,
        //     astraSubWeaponTask,
        //     skillNameTask);
        await gearStringTask;
        await itemOptionTask;
        await astraSubWeaponTask;
        await skillNameTask;

        var gearStringData = await gearStringTask;
        var itemOptionData = await itemOptionTask;
        var astraSubWeaponData = await astraSubWeaponTask;
        var skillNameData = await skillNameTask;

        var data = new SortedDictionary<int, MalibGear>();

        var source = factory.CreateSource();
        var parser = factory.CreateParser(
            gearStringData,
            itemOptionData,
            astraSubWeaponData,
            skillNameData);
        var sink = factory.CreateDictionaryCollector(data);

        source.LinkTo(parser, new DataflowLinkOptions { PropagateCompletion = true });
        parser.LinkTo(sink, new DataflowLinkOptions { PropagateCompletion = true });

        await sink.Completion;

        return data;
    }
}