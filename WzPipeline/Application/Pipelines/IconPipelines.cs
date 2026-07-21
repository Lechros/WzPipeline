using System.Drawing;
using System.Threading.Tasks.Dataflow;
using WzComparerR2;
using WzPipeline.Application.Core;
using WzPipeline.Application.Exporters;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.Item;
using WzPipeline.Domains.Shared.Icon;
using WzPipeline.Wz;

namespace WzPipeline.Application.Pipelines;

public sealed class GearRawIconPipeline(WzTree tree, CancellationToken cancellationToken = default)
    : IconStreamPipeline<GearNode>(PipelineIds.GearRawIcon, tree, static (node, find) => node.GetIconRawNode(find),
        cancellationToken);

public sealed class ItemIconPipeline(WzTree tree, CancellationToken cancellationToken = default)
    : IconStreamPipeline<ItemNode>(PipelineIds.ItemIcon, tree, static (node, find) => node.GetIconNode(find),
        cancellationToken);

public sealed class ItemRawIconPipeline(WzTree tree, CancellationToken cancellationToken = default)
    : IconStreamPipeline<ItemNode>(PipelineIds.ItemRawIcon, tree, static (node, find) => node.GetIconRawNode(find),
        cancellationToken);

public sealed class GearIconOriginPipeline(WzTree tree, CancellationToken cancellationToken = default)
    : IconOriginPipeline<GearNode>(PipelineIds.GearIconOrigin, tree, static (node, find) => node.GetIconNode(find),
        cancellationToken);

public sealed class GearRawIconOriginPipeline(WzTree tree, CancellationToken cancellationToken = default)
    : IconOriginPipeline<GearNode>(PipelineIds.GearRawIconOrigin, tree,
        static (node, find) => node.GetIconRawNode(find), cancellationToken);

public sealed class ItemIconOriginPipeline(WzTree tree, CancellationToken cancellationToken = default)
    : IconOriginPipeline<ItemNode>(PipelineIds.ItemIconOrigin, tree, static (node, find) => node.GetIconNode(find),
        cancellationToken);

public sealed class ItemRawIconOriginPipeline(WzTree tree, CancellationToken cancellationToken = default)
    : IconOriginPipeline<ItemNode>(PipelineIds.ItemRawIconOrigin, tree,
        static (node, find) => node.GetIconRawNode(find), cancellationToken);

public abstract class IconStreamPipeline<TNode> : IPipeline
{
    protected IconStreamPipeline(PipelineId id, WzTree tree,
        Func<TNode, GlobalFindNodeFunction, IconNode?> selector, CancellationToken cancellationToken)
    {
        Id = id;
        var block = new TransformManyBlock<TNode, ImageArtifact>(node =>
        {
            var icon = selector(node, (path, file) => tree.FindNode(path, file)!);
            return icon is null ? [] : [new ImageArtifact(icon.Id, icon.Image)];
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Input = block;
        Output = block;
        Completion = block.Completion;
    }

    public PipelineId Id { get; }
    public ITargetBlock<TNode> Input { get; }
    public ISourceBlock<ImageArtifact> Output { get; }
    public Task Completion { get; }
}

public abstract class IconOriginPipeline<TNode> : IPipeline
{
    protected IconOriginPipeline(PipelineId id, WzTree tree,
        Func<TNode, GlobalFindNodeFunction, IconNode?> selector, CancellationToken cancellationToken)
    {
        Id = id;
        var data = new SortedDictionary<string, Point>();
        var input = new ActionBlock<TNode>(node =>
        {
            var icon = selector(node, (path, file) => tree.FindNode(path, file)!);
            if (icon?.Origin is { } origin) data.TryAdd(icon.Id, origin);
        }, new ExecutionDataflowBlockOptions { CancellationToken = cancellationToken });
        Input = input;
        Completion = input.Completion;
        Result = CompleteAsync();

        async Task<SortedDictionary<string, Point>> CompleteAsync()
        {
            await Completion.ConfigureAwait(false);
            return data;
        }
    }

    public PipelineId Id { get; }
    public ITargetBlock<TNode> Input { get; }
    public Task<SortedDictionary<string, Point>> Result { get; }
    public Task Completion { get; }
}