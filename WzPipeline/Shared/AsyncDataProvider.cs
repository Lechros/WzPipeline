namespace WzPipeline.Shared;

public abstract class AsyncDataProvider<T>
{
    private readonly Lazy<Task<T>> data;

    protected AsyncDataProvider()
    {
        data = new Lazy<Task<T>>(CreateAsync, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public Task<T> GetAsync() => data.Value;

    protected abstract Task<T> CreateAsync();
}