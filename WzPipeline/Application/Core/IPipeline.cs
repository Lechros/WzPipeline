namespace WzPipeline.Application.Core;

public interface IPipeline
{
    public PipelineId Id { get; }
    public Task Completion { get; }
}