using System.Threading.Tasks.Dataflow;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Domains.AstraSubWeapon;

public class AstraSubWeaponBlockFactory(WzTree tree)
{
    public const string Pattern = "Etc/SubWeaponTransferData.img/Job/*";

    public ISourceBlock<SubWeaponTransferNode> CreateSource()
    {
        return tree.MatchNodes(Pattern).ToSourceBlock().Map(node => new SubWeaponTransferNode(node));
    }

    public TransformManyBlock<SubWeaponTransferNode, AstraSubWeaponEntry> CreateConverter()
    {
        return new TransformManyBlock<SubWeaponTransferNode, AstraSubWeaponEntry>(Convert);

        IEnumerable<AstraSubWeaponEntry> Convert(SubWeaponTransferNode node)
        {
            foreach (var targetIds in node.TargetIdGroups)
            {
                var index = 0;
                foreach (var targetId in targetIds)
                {
                    yield return new AstraSubWeaponEntry
                    {
                        Id = targetId,
                        Jobs = [node.Job],
                        Index = index++
                    };
                }
            }
        }
    }

    public ITargetBlock<AstraSubWeaponEntry> CreateDictionaryCollector(
        IDictionary<int, AstraSubWeaponEntry> dictionary)
    {
        return new ActionBlock<AstraSubWeaponEntry>(entry =>
        {
            if (dictionary.TryGetValue(entry.Id, out var prev))
            {
                if (prev.Index != entry.Index)
                {
                    throw new InvalidDataException($"Index is different for {prev.Id}");
                }

                prev.Jobs.Add(entry.Jobs[0]);
            }
            else
            {
                dictionary.Add(entry.Id, entry);
            }
        });
    }
}