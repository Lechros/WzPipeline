namespace WzJson.Common;

public class ProgressReporter<TProgressData>
{
    private readonly IProgress<TProgressData> progress;
    private readonly ProgressDataProducer progressDataProducer;
    private readonly int totalCount;
    private int currentCount;

    public ProgressReporter(IProgress<TProgressData> progress, ProgressDataProducer progressDataProducer,
        int totalCount)
    {
        if (totalCount < 0) throw new ArgumentException("Total count cannot be negative.");
        this.progress = progress;
        this.progressDataProducer = progressDataProducer;
        this.totalCount = totalCount;
        Report(currentCount);
    }

    public int ReportResolution { get; init; } = 100;

    public void Increment()
    {
        var incrementedCount = Interlocked.Increment(ref currentCount);
        if (ShouldReport(incrementedCount))
        {
            Report(incrementedCount);
        }
    }

    public void Complete()
    {
        Report(totalCount);
    }

    private void Report(int count)
    {
        progress.Report(progressDataProducer(count, totalCount));
    }

    private bool ShouldReport(int count)
    {
        if (totalCount == 0) return false;
        if (totalCount < ReportResolution) return true;
        if (count % (totalCount / ReportResolution) == 0) return true;
        return false;
    }

    public delegate TProgressData ProgressDataProducer(int currentCount, int totalCount);
}