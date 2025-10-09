namespace WzPipeline.Core.Stereotype;

public abstract class AbstractExporter<T> : IExporter<T>, IExporter
{
    protected virtual void Prepare()
    {
    }

    void IExporter<T>.Prepare()
    {
        Prepare();
    }

    void IExporter.Prepare()
    {
        Prepare();
    }

    public abstract Task Export(T model);

    public Task Export(object model)
    {
        return Export((T)model);
    }

    public virtual void Cleanup(T model)
    {
    }

    public void Cleanup(object model)
    {
        Cleanup((T)model);
    }
}