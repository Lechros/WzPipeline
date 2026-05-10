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
                if (data.TryGetValue(entry.Id, out var prev))
                {
                    if (prev.Index != entry.Index)
                    {
                        throw new InvalidDataException($"Index is different for {prev.Id}");
                    }
                    prev.Jobs.Add(entry.Jobs[0]);
                }
                else
                {
                    data.Add(entry.Id, entry);
                }
            }
        }

        yield return data;
    }
}