using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using Ninject;
using Spectre.Console;
using Spectre.Console.Rendering;
using WzComparerR2.Common;
using WzJson.Common;
using WzJson.Common.Writer;
using WzJson.Reader;

namespace WzJson;

public static class Program
{
    public static readonly string BaseWzPath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    public static readonly string OutputPath = Path.Join(AppContext.BaseDirectory, @"output\");

    public static void Main(string[] args)
    {
        var kernel = new StandardKernel();
        var jsonSerializer = new JsonSerializer();
        kernel.Bind<IWzProvider>().ToMethod(_ => new WzProvider(BaseWzPath)).InSingletonScope();
        kernel.Bind<GlobalFindNodeFunction>().ToMethod(ctx => ctx.Kernel.Get<IWzProvider>().FindNode);
        kernel.Bind<IWriter>().ToMethod(_ => new JsonFileWriter(OutputPath, jsonSerializer))
            .InSingletonScope();
        kernel.Bind<IWriter>().ToMethod(_ => new PngFilesWriter(OutputPath)).InSingletonScope();

        var sw = new Stopwatch();

        AnsiConsole.MarkupLineInterpolated($"Validating wz from [purple]{BaseWzPath}[/]");
        sw.Restart();
        kernel.Get<IWzProvider>();
        sw.Stop();
        AnsiConsole.MarkupLineInterpolated($"Done in [yellow]{sw.ElapsedMilliseconds}ms[/].");


        AnsiConsole.MarkupLineInterpolated($"Reading wz string data");
        sw.Restart();
        _ = kernel.Get<GlobalStringDataProvider>().GlobalStringData;
        sw.Stop();
        AnsiConsole.MarkupLineInterpolated($"Done in [yellow]{sw.ElapsedMilliseconds}ms[/].");

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
                Choices = ["item icons", "item icon origins"],
                GetReadOptions = choices => new ItemReadOptions
                {
                    ItemIconPath = choices.Contains("item icons") ? "item-icon" : null,
                    ItemIconOriginJsonPath = choices.Contains("item icon origins") ? "item-origin.json" : null
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


        var writers = kernel.GetAll<IWriter>().ToList();
        AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn(),
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
                    var readTask = ctx.AddTask(GetProgressDesc(readDesc, 0, "?"));

                    var reader = (IWzReader)kernel.Get(job.ReaderType);
                    var options = job.GetReadOptions(choices);
                    var readProgress = new Progress<ReadProgressData>(rData =>
                    {
                        readTask.Value = rData.Value;
                        readTask.MaxValue = rData.MaxValue;
                        readTask.Description = GetProgressDesc(readDesc, rData.Value, rData.MaxValue);
                    });
                    datas.AddRange(reader.Read(options, readProgress));
                    readTask.StopTask();
                }

                Parallel.ForEach(datas, data =>
                {
                    var writeDesc = data is ILabeledData labeledData
                        ? $"Writing {labeledData.Label}"
                        : $"Writing ...";
                    var writeTask = ctx.AddTask(GetProgressDesc(writeDesc, 0, "?"));

                    var writer = writers.First(writer => writer.Supports(data));
                    var writeProgress = new Progress<WriteProgressData>(wData =>
                    {
                        writeTask.Value = wData.Value;
                        writeTask.MaxValue = wData.MaxValue;
                        writeTask.Description = GetProgressDesc(writeDesc, wData.Value, wData.MaxValue);
                    });
                    writer.Write(data, writeProgress);
                    writeTask.StopTask();
                });
            });
    }

    private static string GetProgressDesc(string message, dynamic current, dynamic total)
    {
        var currentColor = current.ToString() == total.ToString() ? "green" : "yellow";
        return $"{message} []([/][{currentColor}]{current}[/][]/{total})[/]";
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