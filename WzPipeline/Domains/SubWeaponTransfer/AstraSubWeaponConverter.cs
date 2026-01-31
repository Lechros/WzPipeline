using WzPipeline.Core.Stereotype;

namespace WzPipeline.Domains.SubWeaponTransfer;

public class AstraSubWeaponConverter : AbstractConverter<ISubWeaponTransferNode, List<AstraSubWeaponEntry>>
{
    public override List<AstraSubWeaponEntry> Convert(ISubWeaponTransferNode node)
    {
        var entries = new List<AstraSubWeaponEntry>();
        foreach (var targetIds in node.TargetIdGroups)
        {
            var job = node.Job;
            var index = 0;
            foreach (var targetId in targetIds)
            {
                entries.Add(new AstraSubWeaponEntry()
                {
                    Id = targetId,
                    Index = index++,
                    Job = job
                });
            }
        }
        return entries;
    }
}