using System.Threading.Tasks.Dataflow;
using WzComparerR2.WzLib;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Application.Gear;

public class AstraSubWeaponDataBuilder(WzTree tree, AstraSubWeaponParser parser)
{
    public async Task<AstraSubWeaponData> BuildAsync()
    {
        var data = new AstraSubWeaponData();

        var source = tree.MatchNodes(AstraSubWeaponSource.Pattern).ToSourceBlock();
        var wrap = new TransformBlock<Wz_Node, SubWeaponTransferNode>(node => new SubWeaponTransferNode(node));
        var parse = new TransformManyBlock<SubWeaponTransferNode, AstraSubWeaponOccurrence>(node => parser.Parse(node));
        var collector = new ActionBlock<AstraSubWeaponOccurrence>(occurrence => AddOccurence(data, occurrence),
            new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

        source.LinkTo(wrap, new DataflowLinkOptions { PropagateCompletion = true });
        wrap.LinkTo(parse, new DataflowLinkOptions { PropagateCompletion = true });
        parse.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        await collector.Completion;
        return data;
    }

    private static void AddOccurence(IDictionary<int, AstraSubWeaponEntry> dictionary,
        AstraSubWeaponOccurrence occurrence)
    {
        if (!dictionary.TryGetValue(occurrence.Id, out var existing))
        {
            dictionary.Add(occurrence.Id, new AstraSubWeaponEntry
            {
                Id = occurrence.Id, Index = occurrence.Index, Jobs = [occurrence.Job]
            });
            return;
        }

        if (existing.Index != occurrence.Index)
            throw new InvalidDataException(
                $"Astra sub-weapon '{occurrence.Id}' has conflicting indices: " +
                $"{existing.Index} and {occurrence.Index}.");

        existing.Jobs.Add(occurrence.Job);
    }
}