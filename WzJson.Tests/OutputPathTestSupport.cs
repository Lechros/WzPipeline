namespace WzJson.Tests;

public abstract class OutputPathTestSupport
{
    protected string OutputPath = "";

    [TestInitialize]
    public void setup_outputPath()
    {
        string path;
        do
        {
            path = Guid.NewGuid().ToString();
        } while (Directory.Exists(path));

        OutputPath = path + Path.DirectorySeparatorChar;
    }

    [TestCleanup]
    public void remove_outputPath()
    {
        // Safety checks to not delete the wrong directory.
        Assert.AreEqual(OutputPath.Length, 37, "Output path is not GUID but was: " + OutputPath);
        Assert.IsFalse(OutputPath[..36].Contains(Path.DirectorySeparatorChar),
            "Output path contains invalid separator: " + OutputPath);

        if (Directory.Exists(OutputPath)) Directory.Delete(OutputPath, true);
    }
}