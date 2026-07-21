using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Domains.AstraSubWeapon;

namespace WzPipeline.Application.Pipelines;

public sealed class AstraSubWeaponDataPipeline : IPipeline
{
    public AstraSubWeaponDataPipeline(
        AstraSubWeaponParser parser,
        CancellationToken cancellationToken = default)
    {
        var data = new AstraSubWeaponData();
        var input = new ActionBlock<SubWeaponTransferNode>(node =>
        {
            foreach (var occurrence in parser.Parse(node)) AddOccurrence(data, occurrence);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Input = input;
        Completion = input.Completion;
        Result = CompleteAsync();

        async Task<AstraSubWeaponData> CompleteAsync()
        {
            await Completion.ConfigureAwait(false);
            return data;
        }
    }

    public PipelineId Id => PipelineIds.AstraSubWeaponData;
    public ITargetBlock<SubWeaponTransferNode> Input { get; }
    public Task<AstraSubWeaponData> Result { get; }
    public Task Completion { get; }

    private static void AddOccurrence(
        IDictionary<int, AstraSubWeaponEntry> data,
        AstraSubWeaponOccurrence occurrence)
    {
        if (!data.TryGetValue(occurrence.Id, out var existing))
        {
            data.Add(occurrence.Id, new AstraSubWeaponEntry
            {
                Id = occurrence.Id,
                Index = occurrence.Index,
                Jobs = [occurrence.Job]
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