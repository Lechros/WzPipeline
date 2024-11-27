namespace WzJson;

public abstract class AbstractFileExporter : IExporter
{
    protected AbstractFileExporter(string outputPath)
    {
        OutputPath = outputPath;
        if (!Path.EndsInDirectorySeparator(OutputPath))
        {
            throw new ArgumentException("OutputPath must be path but was: " + outputPath);
        }
    }

    protected string OutputPath { get; }

    public abstract bool Supports<T>(IData<T> data);

    public void Export<T>(IData<T> data)
    {
        if (!Supports(data))
        {
            throw new NotSupportedException("Cannot export data of type " + data.GetType().Name);
        }

        EnsureDirectory(OutputPath);
        ExportItems(data);
    }

    protected abstract void ExportItems<T>(IData<T> data);

    protected void EnsureDirectory(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path: " + path));
    }
}