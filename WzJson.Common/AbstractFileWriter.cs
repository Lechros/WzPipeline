namespace WzJson.Common;

public abstract class AbstractFileWriter : IWriter
{
    protected AbstractFileWriter(string outputPath)
    {
        OutputPath = outputPath;
        if (!Path.EndsInDirectorySeparator(OutputPath))
            throw new ArgumentException("OutputPath must be path but was: " + outputPath);
    }

    protected string OutputPath { get; }

    public abstract bool Supports(IData data);

    public void Write(IData data)
    {
        if (!Supports(data))
            throw new NotSupportedException("Cannot export data of type " + data.GetType().Name);

        EnsureDirectory(OutputPath);
        WriteItems(data);
    }

    protected abstract void WriteItems(IData data);

    protected void EnsureDirectory(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path: " + path));
    }
}