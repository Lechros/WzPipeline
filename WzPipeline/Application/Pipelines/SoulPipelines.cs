using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Shared.String;
using WzPipeline.Domains.Soul;

namespace WzPipeline.Application.Pipelines;

public sealed class ConsumeNameDataPipeline : IPipeline
{
    private readonly Dictionary<string, string> data = [];

    public ConsumeNameDataPipeline(CancellationToken cancellationToken = default)
    {
        Input = new ActionBlock<StringNode>(node =>
        {
            if (node.Name is not null) data.TryAdd(node.Key, node.Name);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Completion = Input.Completion;
        Result = CompleteAsync();
    }

    public PipelineId Id => PipelineIds.ConsumeNameData;
    public ITargetBlock<StringNode> Input { get; }
    public Task Completion { get; }
    public Task<Dictionary<string, string>> Result { get; }

    private async Task<Dictionary<string, string>> CompleteAsync()
    {
        await Completion;
        return data;
    }
}

public sealed class SoulInfoDataPipeline : IPipeline
{
    private readonly Dictionary<int, SoulInfo> data = [];

    public SoulInfoDataPipeline(SoulCollectionParser parser, CancellationToken cancellationToken = default)
    {
        Input = new ActionBlock<SoulCollectionNode>(node =>
        {
            foreach (var x in parser.Parse(node)) data.TryAdd(x.SoulId, x);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Completion = Input.Completion;
        Result = CompleteAsync();
    }

    public PipelineId Id => PipelineIds.SoulInfoData;
    public ITargetBlock<SoulCollectionNode> Input { get; }
    public Task Completion { get; }
    public Task<Dictionary<int, SoulInfo>> Result { get; }

    private async Task<Dictionary<int, SoulInfo>> CompleteAsync()
    {
        await Completion;
        return data;
    }
}

public sealed class SkillOptionDataPipeline : IPipeline
{
    private readonly Dictionary<int, IList<SkillOption>> data = [];

    public SkillOptionDataPipeline(SkillOptionParser parser, Task<ItemOptionData> itemOptions,
        CancellationToken cancellationToken = default)
    {
        Input = new ActionBlock<SkillOptionNode>(async node =>
        {
            var context = new SkillOptionParseContext { ItemOptionData = await itemOptions.ConfigureAwait(false) };
            foreach (var x in parser.Parse(node, context))
            {
                if (!data.TryGetValue(x.SkillId, out var list)) data.Add(x.SkillId, list = []);
                list.Add(x);
            }
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Completion = Input.Completion;
        Result = CompleteAsync();
    }

    public PipelineId Id => PipelineIds.SkillOptionData;
    public ITargetBlock<SkillOptionNode> Input { get; }
    public Task Completion { get; }
    public Task<Dictionary<int, IList<SkillOption>>> Result { get; }

    private async Task<Dictionary<int, IList<SkillOption>>> CompleteAsync()
    {
        await Completion;
        return data;
    }
}

public sealed class SoulDataPipeline : IPipeline
{
    private readonly SortedDictionary<int, MalibSoul> data = [];

    public SoulDataPipeline(SoulParser parser, Task<Dictionary<string, string>> consume,
        Task<Dictionary<string, string>> skills, Task<Dictionary<int, SoulInfo>> info,
        Task<Dictionary<int, IList<SkillOption>>> options, CancellationToken cancellationToken = default)
    {
        Input = new ActionBlock<SoulNode>(async node =>
        {
            var context = new SoulParseContext
            {
                ConsumeNameData = await consume, SkillNameData = await skills, SoulInfoData = await info,
                SkillOptionData = await options
            };
            foreach (var x in parser.Parse(node, context)) data.TryAdd(x.Id, x);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Completion = Input.Completion;
        Result = CompleteAsync();
    }

    public PipelineId Id => PipelineIds.SoulData;
    public ITargetBlock<SoulNode> Input { get; }
    public Task Completion { get; }
    public Task<SortedDictionary<int, MalibSoul>> Result { get; }

    private async Task<SortedDictionary<int, MalibSoul>> CompleteAsync()
    {
        await Completion;
        return data;
    }
}