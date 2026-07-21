using Spectre.Console;
using WzPipeline.Application.Core;
using WzPipeline.Application.Pipelines;

namespace WzPipeline.Application.Configuration;

public static class PipelineSelection
{
    private const string AllTarget = "(ALL)";
    private static readonly IReadOnlyDictionary<string, PipelineId> Choices =
        new Dictionary<string, PipelineId>(StringComparer.OrdinalIgnoreCase)
        {
            [PipelineIds.GearData.Value] = PipelineIds.GearData,
            [PipelineIds.GearIcon.Value] = PipelineIds.GearIcon,
            [PipelineIds.GearIconOrigin.Value] = PipelineIds.GearIconOrigin,
            [PipelineIds.GearRawIcon.Value] = PipelineIds.GearRawIcon,
            [PipelineIds.GearRawIconOrigin.Value] = PipelineIds.GearRawIconOrigin,
            [PipelineIds.ItemIcon.Value] = PipelineIds.ItemIcon,
            [PipelineIds.ItemIconOrigin.Value] = PipelineIds.ItemIconOrigin,
            [PipelineIds.ItemRawIcon.Value] = PipelineIds.ItemRawIcon,
            [PipelineIds.ItemRawIconOrigin.Value] = PipelineIds.ItemRawIconOrigin,
            [PipelineIds.SetItemData.Value] = PipelineIds.SetItemData,
            [PipelineIds.ExclusiveEquipData.Value] = PipelineIds.ExclusiveEquipData,
            [PipelineIds.SoulData.Value] = PipelineIds.SoulData
        };

    public static IReadOnlyCollection<PipelineId> Select(string[] args)
    {
        if (args.Length > 0)
            return args.Select(name =>
                Choices.TryGetValue(name, out var id)
                    ? id
                    : throw new ArgumentException($"Unknown pipeline '{name}'.", nameof(args))).ToArray();

        var selected = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Select pipelines to execute")
            .AddChoices([AllTarget, .. Choices.Keys]));
        if (selected.Contains(AllTarget, StringComparer.Ordinal))
            return Choices.Values.ToArray();

        return selected.Select(name => Choices[name]).ToArray();
    }
}
