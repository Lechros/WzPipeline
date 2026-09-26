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
            foreach (var info in parser.Parse(node))
            {
                foreach (var soulId in info.SoulIds)
                {
                    data.Add(soulId, info);
                }
            }
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
    private readonly Dictionary<int, SkillOption> data = [];

    public SkillOptionDataPipeline(SkillOptionParser parser, Task<ItemOptionData> itemOptions,
        CancellationToken cancellationToken = default)
    {
        Input = new ActionBlock<SkillOptionNode>(async node =>
        {
            var context = new SkillOptionParseContext { ItemOptionData = await itemOptions.ConfigureAwait(false) };
            foreach (var x in parser.Parse(node, context))
            {
                data.Add(x.SkillOptionId, x);
            }
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Completion = Input.Completion;
        Result = CompleteAsync();
    }

    public PipelineId Id => PipelineIds.SkillOptionData;
    public ITargetBlock<SkillOptionNode> Input { get; }
    public Task Completion { get; }
    public Task<Dictionary<int, SkillOption>> Result { get; }

    private async Task<Dictionary<int, SkillOption>> CompleteAsync()
    {
        await Completion;
        return data;
    }
}

public sealed class SoulAmplifyDataPipeline : IPipeline
{
    private readonly HashSet<int> data = [];

    public SoulAmplifyDataPipeline(CancellationToken cancellationToken = default)
    {
        Input = new ActionBlock<SoulAmplifyNode>(node => data.UnionWith(node.UpgradableSoul),
            new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Completion = Input.Completion;
        Result = CompleteAsync();
    }

    public PipelineId Id => PipelineIds.SoulAmplifyData;
    public ITargetBlock<SoulAmplifyNode> Input { get; }
    public Task Completion { get; }
    public Task<HashSet<int>> Result { get; }

    private async Task<HashSet<int>> CompleteAsync()
    {
        await Completion;
        return data;
    }
}

public sealed class SoulDataPipeline : IPipeline
{
    private readonly SortedDictionary<int, MalibRawSoul> data = [];

    public SoulDataPipeline(SoulParser parser, Task<Dictionary<string, string>> consume,
        Task<Dictionary<int, SoulInfo>> info, Task<Dictionary<int, SkillOption>> options,
        Task<HashSet<int>> amplify,
        CancellationToken cancellationToken = default)
    {
        Input = new ActionBlock<SoulNode>(async node =>
        {
            var context = new SoulParseContext
            {
                ConsumeNameData = await consume, SoulInfoData = await info,
                SkillOptionData = await options, SoulAmplifyData = await amplify
            };
            foreach (var x in parser.Parse(node, context)) data.TryAdd(x.Id, x);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Completion = Input.Completion;
        Result = CompleteAsync();
    }

    public PipelineId Id => PipelineIds.SoulData;
    public ITargetBlock<SoulNode> Input { get; }
    public Task Completion { get; }
    public Task<SortedDictionary<int, MalibRawSoul>> Result { get; }

    private async Task<SortedDictionary<int, MalibRawSoul>> CompleteAsync()
    {
        await Completion;
        return data;
    }
}