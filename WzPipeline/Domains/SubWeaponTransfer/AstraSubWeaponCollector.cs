using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.SubWeaponTransfer;

public class AstraSubWeaponCollector : AbstractProcessor<List<AstraSubWeaponEntry>, AstraSubWeaponData>
{
    public override IEnumerable<AstraSubWeaponData> Process(IEnumerable<List<AstraSubWeaponEntry>> models)
    {
        var data = new AstraSubWeaponData();
        foreach (var entries in models)
        {
            foreach (var entry in entries)
            {
                data.TryAdd(entry.Id, entry);
            }
        }

        yield return data;
    }
}