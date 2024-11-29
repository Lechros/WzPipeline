namespace WzJson.Common.Tests;

public abstract class OutputPathTestSupport : IDisposable
{
    protected readonly string OutputPath;

    protected OutputPathTestSupport()
    {
        string path;
        do
        {
            path = Guid.NewGuid().ToString();
        } while (Directory.Exists(path));

        OutputPath = path + Path.DirectorySeparatorChar;
    }

    public void Dispose()
    {
        // Safety checks to not delete the wrong directory.
        Assert.Equal(37, OutputPath.Length);
        Assert.DoesNotContain(Path.DirectorySeparatorChar.ToString(), OutputPath[..36]);

        if (Directory.Exists(OutputPath)) Directory.Delete(OutputPath, true);
    }
}