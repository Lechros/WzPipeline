namespace WzJson.Core.Stereotype;

public interface IExporter
{
    public void Prepare();
    public Task Export(object model);
    
    public void Cleanup(object model);
}

public interface IExporter<in T>
{
    public void Prepare();
    public Task Export(T model);
    public void Cleanup(T model);
}