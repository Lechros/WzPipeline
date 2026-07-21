using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Application.Shared.Dataflow;
using WzPipeline.Domains.AstraSubWeapon;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.Shared.ItemOption;
using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Application.Pipelines;

public sealed class GearDataPipeline : IPipeline
{
    public PipelineId Id => PipelineIds.GearData;
    public ITargetBlock<GearNode> Input { get; }
    public Task<SortedDictionary<int, MalibGear>> Result { get; }
    public Task Completion { get; }

    public GearDataPipeline(
        GearParser parser,
        Task<ItemOptionData> itemOptionData,
        Task<Dictionary<string, NameDesc>> gearStringData,
        Task<Dictionary<string, string>> skillNameData,
        Task<AstraSubWeaponData> astraSubWeaponData,
        CancellationToken cancellationToken = default)
    {
        var contextTask = CreateContextAsync(
            itemOptionData, gearStringData, skillNameData, astraSubWeaponData);

        var parserBlock = new TransformManyBlock<GearNode, MalibGear>(async node =>
        {
            var context = await contextTask.ConfigureAwait(false);
            return parser.Parse(node, context);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });

        var data =
            new SortedDictionary<int, MalibGear>();
        var collectorBlock =
            DataflowCollectors.DictionaryCollector(data, static entry => entry.Id, cancellationToken);

        parserBlock.LinkTo(collectorBlock, new DataflowLinkOptions { PropagateCompletion = true });

        Input = parserBlock;
        Completion = collectorBlock.Completion;
        Result = GetDataAsync();

        return;

        static async Task<GearParseContext> CreateContextAsync(
            Task<ItemOptionData> itemOptionDataTask,
            Task<Dictionary<string, NameDesc>> gearStringDataTask,
            Task<Dictionary<string, string>> skillNameDataTask,
            Task<AstraSubWeaponData> astraSubWeaponDataTask)
        {
            return new GearParseContext
            {
                ItemOptionData = await itemOptionDataTask.ConfigureAwait(false),
                GearStringData = await gearStringDataTask.ConfigureAwait(false),
                SkillNameData = await skillNameDataTask.ConfigureAwait(false),
                AstraSubWeaponData = await astraSubWeaponDataTask.ConfigureAwait(false)
            };
        }

        async Task<SortedDictionary<int, MalibGear>> GetDataAsync()
        {
            await collectorBlock.Completion.ConfigureAwait(false);
            return data;
        }
    }
}