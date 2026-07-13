namespace WzPipeline.Domains.AstraSubWeapon;

public class AstraSubWeaponParser
{
    public IEnumerable<AstraSubWeaponOccurrence> Parse(SubWeaponTransferNode node)
    {
        foreach (var targetIds in node.TargetIdGroups)
        {
            var index = 0;
            foreach (var targetId in targetIds)
            {
                yield return new AstraSubWeaponOccurrence
                {
                    Id = targetId,
                    Job = node.Job,
                    Index = index++
                };
            }
        }
    }
}