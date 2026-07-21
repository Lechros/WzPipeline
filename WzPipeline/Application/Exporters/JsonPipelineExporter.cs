using WzPipeline.Application.Core;
using WzPipeline.Application.Core.Reporting;
using WzPipeline.Application.Shared.Export;

namespace WzPipeline.Application.Exporters;

public sealed class JsonPipelineExporter(JsonFileExporter exporter)
{
    public async Task ExportAsync<T>(
        Task<T> result,
        PipelineExportContext context)
    {
        var value = await result.ConfigureAwait(false);
        await exporter.ExportAsync(
            value, context.GetOutputPath(), context.CancellationToken).ConfigureAwait(false);
        context.Reporter.Report(new ExecutionReport(
            ReportOperation.Export, context.PipelineId.Value, ReportStatus.Running, 1));
    }
}