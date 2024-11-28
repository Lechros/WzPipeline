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

    public abstract bool Supports(IData data);

    public void Export(IData data)
    {
        if (!Supports(data))
        {
            throw new NotSupportedException("Cannot export data of type " + data.GetType().Name);
        }

        EnsureDirectory(OutputPath);
        ExportItems(data);
    }

    protected abstract void ExportItems(IData data);

    protected void EnsureDirectory(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path: " + path));
    }
}