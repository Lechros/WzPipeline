using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Application.Exporters;
using WzPipeline.Domains.Gear;
using WzPipeline.Wz;

namespace WzPipeline.Application.Pipelines;

public sealed class GearIconPipeline : IPipeline
{
    public GearIconPipeline(WzTree tree, CancellationToken cancellationToken = default)
    {
        var converter = new TransformManyBlock<GearNode, ImageArtifact>(node =>
        {
            if (node.Id is null) return [];
            var icon = node.GetIconNode((path, file) => tree.FindNode(path, file)!);
            return icon is null ? [] : [new ImageArtifact(icon.Id, icon.Image)];
        }, new ExecutionDataflowBlockOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = 1
        });

        Input = converter;
        Output = converter;
        Completion = converter.Completion;
    }

    public PipelineId Id => PipelineIds.GearIcon;
    public ITargetBlock<GearNode> Input { get; }
    public ISourceBlock<ImageArtifact> Output { get; }
    public Task Completion { get; }
}