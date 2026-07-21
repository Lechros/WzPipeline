using System.Threading.Tasks.Dataflow;
using WzPipeline.Application.Core;
using WzPipeline.Application.Core.Reporting;
using WzPipeline.Application.Shared.Export;

namespace WzPipeline.Application.Exporters;

public sealed class ImagePipelineExporter(ImageFileExporter exporter)
{
    public Task AttachAsync(
        ISourceBlock<ImageArtifact> source,
        PipelineExportContext context,
        string extension = "png")
    {
        var outputDirectory = context.GetOutputPath();
        long count = 0;
        var target = new ActionBlock<ImageArtifact>(async artifact =>
        {
            try
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                var relativeDirectory = Path.GetRelativePath(
                    context.OutputRootPath, outputDirectory);
                var filename = context.ResolvePath(
                    Path.Combine(relativeDirectory, $"{artifact.Id}.{extension}"));
                await exporter.ExportImageAsync(
                    artifact.Image, filename, context.CancellationToken).ConfigureAwait(false);
                var current = Interlocked.Increment(ref count);
                context.Reporter.Report(new ExecutionReport(
                    ReportOperation.Export, context.PipelineId.Value,
                    ReportStatus.Running, current));
            }
            finally
            {
                artifact.Dispose();
            }
        }, new ExecutionDataflowBlockOptions
        {
            CancellationToken = context.CancellationToken,
            MaxDegreeOfParallelism = 1
        });

        // If the file target faults and declines later artifacts, dispose them rather
        // than leaving Image instances queued without an owner.
        var disposer = new ActionBlock<ImageArtifact>(artifact => artifact.Dispose());
        source.LinkTo(target, new DataflowLinkOptions { PropagateCompletion = true });
        source.LinkTo(disposer, new DataflowLinkOptions { PropagateCompletion = true });

        return CompleteAsync();

        async Task CompleteAsync()
        {
            await target.Completion.ConfigureAwait(false);
            await disposer.Completion.ConfigureAwait(false);
            context.Reporter.Report(new ExecutionReport(
                ReportOperation.Export, context.PipelineId.Value,
                ReportStatus.Running, count));
        }
    }
}