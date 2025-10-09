using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using WzJson.Application;
using WzJson.Application.DependencyInjection;
using WzJson.Cli.Reporting;
using WzJson.Core.Pipeline.Runner;
using WzJson.Shared;

namespace WzJson.Cli;

public static class Program
{
    private const string BaseWzPath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    private const string OutputRootPath = "new-output";

    private static readonly IAnsiConsole Console = AnsiConsole.Console;

    public static void Main(string[] args)
    {
        var sw = new Stopwatch();

        // Load wz
        sw.Restart();
        var wzProvider = new WzProvider(BaseWzPath);
        sw.Stop();
        Console.MarkupLineInterpolated($"Load wz done in {sw.ElapsedMilliseconds}ms");

        // Setup DI
        var services = new ServiceCollection();
        services.AddSingleton<IPipelineRunner, ConsoleProgressReportingPipelineRunner>();

        // Setup builder
        var workflow = new Workflow(services, wzProvider);

        // Get Inputs
        var root = Path.GetFullPath(OutputRootPath);
        var (prompt, actions) = GetPrompt(root);
        var choices = Console.Prompt(prompt);

        // Configure workflow from inputs
        foreach (var choice in choices)
        {
            actions[choice](workflow);
        }

        // Build DI
        var serviceProvider = services.BuildServiceProvider();

        // Run jobs
        sw.Restart();
        workflow.Run(serviceProvider);
        sw.Stop();
        Console.MarkupLineInterpolated($"All jobs completed in [blue]{sw.Elapsed.TotalSeconds:0.0}s[/]");
        Console.Write("Output root path: ");
        Console.Write(new TextPath(root));
        Console.WriteLine();
    }

    private static (MultiSelectionPrompt<string>, IReadOnlyDictionary<string, Action<Workflow>>)
        GetPrompt(string root)
    {
        var prompt = new MultiSelectionPrompt<string>()
            .Title("Select jobs to export")
            .PageSize(10);
        var all = prompt.AddChoice("(ALL)");
        var actions = new Dictionary<string, Action<Workflow>>();
        foreach (var (groupName, tuples) in GetChoices(root))
        {
            var group = all.AddChild(groupName);
            foreach (var (name, configure) in tuples)
            {
                group.AddChild(name);
                actions.Add(name, configure);
            }
        }

        return (prompt, actions);
    }

    private static Dictionary<string, List<(string name, Action<Workflow> configure)>> GetChoices(string root)
    {
        return new Dictionary<string, List<(string, Action<Workflow>)>>
        {
            ["Gear"] =
            [
                ("GearData", w => w.AddGearDataJob(Path.Join(root, "gear.json"))),
                ("GearIcon", w => w.AddGearIconJob(Path.Join(root, "gear-icon"))),
                ("GearIconOrigin", w => w.AddGearIconOriginJob(Path.Join(root, "gear-icon-origin.json"))),
                ("GearRawIcon", w => w.AddGearRawIconJob(Path.Join(root, "gear-raw-icon"))),
                ("GearRawIconOrigin", w => w.AddGearRawIconOriginJob(Path.Join(root, "gear-raw-icon-origin.json")))
            ],
            ["Item"] =
            [
                ("ItemIcon", w => w.AddItemIconJob(Path.Join(root, "item-icon"))),
                ("ItemIconOrigin", w => w.AddItemIconOriginJob(Path.Join(root, "item-icon-origin.json"))),
                ("ItemRawIcon", w => w.AddItemRawIconJob(Path.Join(root, "item-raw-icon"))),
                ("ItemRawIconOrigin", w => w.AddItemRawIconOriginJob(Path.Join(root, "item-raw-icon-origin.json")))
            ],
            ["SetItem"] =
            [
                ("SetItemData", w => w.AddSetItemDataJob(Path.Join(root, "set-item.json")))
            ],
            ["Soul"] =
            [
                ("SoulData", w => w.AddSoulDataJob(Path.Join(root, "soul.json")))
            ]
        };
    }
}