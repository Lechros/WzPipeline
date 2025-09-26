namespace WzJson.V2.Core.Stereotype;

public interface IExporter
{
    public void Prepare();
    public Task Export(object model);
}

public interface IExporter<in T>
{
    public void Prepare();
    public Task Export(T model);
}