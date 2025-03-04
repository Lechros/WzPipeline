using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using Ninject;
using Spectre.Console;
using Spectre.Console.Rendering;
using WzComparerR2.Common;
using WzJson.Common;
using WzJson.Common.Writer;
using WzJson.DataProvider;
using WzJson.Reader;

namespace WzJson;

public static class Program
{
    public static readonly string DefaultBaseWzPath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    public static readonly string OutputPath = Path.Join(AppContext.BaseDirectory, @"output\");

    public static void Main(string[] args)
    {
        var baseWzPath = DefaultBaseWzPath;

        AnsiConsole.MarkupLineInterpolated($"Base.wz path is set to [purple]{baseWzPath}[/]");
        var shouldChangeBaseWzPath = AnsiConsole.Prompt(
            new ConfirmationPrompt("Would you like to change the path?") { DefaultValue = false });
        if (shouldChangeBaseWzPath)
        {
            baseWzPath = AnsiConsole.Ask<string>("Input Base.wz path:");
        }

        var kernel = new StandardKernel();
        var jsonSerializer = new JsonSerializer();
        kernel.Bind<IWzProvider>().ToMethod(_ => new WzProvider(baseWzPath)).InSingletonScope();
        kernel.Bind<GlobalFindNodeFunction>().ToMethod(ctx => ctx.Kernel.Get<IWzProvider>().FindNode);
        kernel.Bind<IWriter>().ToMethod(_ => new JsonFileWriter(OutputPath, jsonSerializer))
            .InSingletonScope();
        kernel.Bind<IWriter>().ToMethod(_ => new PngFilesWriter(OutputPath)).InSingletonScope();

        var sw = new Stopwatch();

        AnsiConsole.MarkupLineInterpolated($"Validating wz from [purple]{baseWzPath}[/]");
        sw.Restart();
        kernel.Get<IWzProvider>();
        sw.Stop();
        AnsiConsole.MarkupLine($"Done in {ToSecondsString(sw)}.");


        AnsiConsole.MarkupLineInterpolated($"Reading wz string data");
        sw.Restart();
        _ = kernel.Get<GlobalStringDataProvider>().Data;
        sw.Stop();
        AnsiConsole.MarkupLine($"Done in {ToSecondsString(sw)}.");

        AnsiConsole.MarkupLineInterpolated($"Output directory set to [purple]{Path.GetFullPath(OutputPath)}[/]");

        List<Job> jobs =
        [
            new("gear", typeof(GearReader))
            {
                Choices = ["gear data", "gear icons", "gear icon origins", "gear raw icons", "gear raw icon origins"],
                GetReadOptions = choices => new GearReadOptions
                {
                    GearDataJsonPath = choices.Contains("gear data") ? "gear-data.json" : null,
                    GearIconPath = choices.Contains("gear data") ? "gear-icon" : null,
                    GearIconOriginJsonPath = choices.Contains("gear icon origins") ? "gear-origin.json" : null,
                    GearIconRawPath = choices.Contains("gear raw icons") ? "gear-raw-icon" : null,
                    GearIconRawOriginJsonPath =
                        choices.Contains("gear raw icon origins") ? "gear-raw-origin.json" : null
                }
            },
            new("item option", typeof(ItemOptionReader))
            {
                Choices = ["item option data"],
                GetReadOptions = choices => new ItemOptionReadOptions
                {
                    ItemOptionJsonPath = choices.Contains("item option data") ? "item-option.json" : null
                }
            },
            new("soul", typeof(SoulReader))
            {
                Choices = ["soul data"],
                GetReadOptions = choices => new SoulReadOptions
                {
                    SoulDataJsonPath = choices.Contains("soul data") ? "soul-data.json" : null
                }
            },
            new("set item", typeof(SetItemReader))
            {
                Choices = ["set item data"],
                GetReadOptions = choices => new SetItemReadOptions
                {
                    SetItemJsonName = choices.Contains("set item data") ? "set-item.json" : null
                }
            },
            new("item", typeof(ItemReader))
            {
                Choices = ["item icons", "item icon origins", "item raw icons", "item raw icon origins"],
                GetReadOptions = choices => new ItemReadOptions
                {
                    ItemIconPath = choices.Contains("item icons") ? "item-icon" : null,
                    ItemIconOriginJsonPath = choices.Contains("item icon origins") ? "item-origin.json" : null,
                    ItemIconRawPath = choices.Contains("item raw icons") ? "item-raw-icon" : null,
                    ItemIconRawOriginJsonPath =
                        choices.Contains("item raw icon origins") ? "item-raw-origin.json" : null
                }
            },
            new("skill", typeof(SkillReader))
            {
                Choices = ["skill icons"],
                GetReadOptions = choices => new SkillReadOptions
                {
                    SkillIconPath = choices.Contains("skill icons") ? "skill-icon" : null
                }
            }
        ];

        var multiSelectionPrompt = new MultiSelectionPrompt<string>().Title("Select items to export").PageSize(15);
        var allChoice = multiSelectionPrompt.AddChoice("(All)");
        foreach (var job in jobs)
        {
            switch (job.Choices.Count)
            {
                case > 1:
                    var groupItem = allChoice.AddChild(job.Name);
                    foreach (var choice in job.Choices)
                        groupItem.AddChild(choice);
                    break;
                case 1:
                    allChoice.AddChild(job.Choices[0]);
                    break;
                default:
                    throw new DataException("Empty choices for job: " + job.Name);
            }
        }

        var choices = AnsiConsole.Prompt(multiSelectionPrompt);

        sw.Restart();
        var writers = kernel.GetAll<IWriter>().ToList();
        AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn { Alignment = Justify.Left },
                new RatioColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new ElapsedSecondsColumn(),
                new SpinnerColumn())
            .Start(ctx =>
            {
                List<IData> datas = [];
                foreach (var job in jobs)
                {
                    if (!job.ShouldRun(choices)) continue;

                    var readDesc = $"Reading {job.Name}";
                    var readTask = ctx.AddTask(readDesc);

                    var reader = (IWzReader)kernel.Get(job.ReaderType);
                    var options = job.GetReadOptions(choices);
                    var readProgress = new Progress<ReadProgressData>(rData =>
                    {
                        readTask.Value = rData.Value;
                        readTask.MaxValue = rData.MaxValue;
                    });
                    datas.AddRange(reader.Read(options, readProgress));
                    readTask.StopTask();
                }

                var writeTasks = datas.Select(data =>
                {
                    var writeDesc = data is ILabeled labeled
                        ? $"Writing {labeled.Label}"
                        : $"Writing ...";
                    return ctx.AddTask(writeDesc);
                }).ToList();
                Parallel.ForEach(Enumerable.Range(0, datas.Count), i =>
                {
                    var data = datas[i];
                    var writeTask = writeTasks[i];
                    var writer = writers.First(writer => writer.Supports(data));
                    var writeProgress = new Progress<WriteProgressData>(wData =>
                    {
                        writeTask.Value = wData.Value;
                        writeTask.MaxValue = wData.MaxValue;
                    });
                    writer.Write(data, writeProgress);
                    writeTask.StopTask();
                });

                foreach (var data in datas)
                {
                    if (data is IDisposable disposable)
                        disposable.Dispose();
                }
            });

        sw.Stop();
        AnsiConsole.MarkupLine($"Done in {ToSecondsString(sw)}.");
    }

    private static string ToSecondsString(Stopwatch stopwatch)
    {
        return $@"[yellow]{stopwatch.Elapsed:s\.f}s[/]";
    }

    public class Job(string name, Type readerType)
    {
        public string Name { get; } = name;
        public Type ReaderType { get; } = readerType;
        public required List<string> Choices { get; init; }
        public required Func<List<string>, IReadOptions> GetReadOptions { get; init; }

        public bool ShouldRun(List<string> choices)
        {
            return choices.Intersect(Choices).Any();
        }
    }

    public sealed class RatioColumn : ProgressColumn
    {
        protected override bool NoWrap => true;

        public Style CompletedStyle { get; set; } = Color.Yellow;

        public Style FinishedStyle { get; set; } = Color.Green;

        public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
        {
            return new Columns(
                new Text($"{task.Value}", task.IsFinished ? FinishedStyle : CompletedStyle).Justify(Justify.Right),
                new Text("/"),
                new Text($"{task.MaxValue}").Justify(Justify.Right)
            );
        }
    }

    public sealed class ElapsedSecondsColumn : ProgressColumn
    {
        protected override bool NoWrap => true;

        public Style Style { get; set; } = Color.Blue;

        public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
        {
            var elapsed = task.ElapsedTime;
            if (elapsed == null)
            {
                return new Markup("--");
            }

            return new Text($@"{elapsed.Value:s\.f}s", Style);
        }

        public override int? GetColumnWidth(RenderOptions options)
        {
            return 5;
        }
    }
}