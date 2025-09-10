namespace WzJson.Common.V2;

public interface IExporter<in T>
{
    public void Export(T model, string path);
}