using FluentAssertions;

namespace WzJson.Common.Tests;

public abstract class OutputPathTestSupport
{
    protected string OutputPath;

    [OneTimeSetUp]
    public void SetUp()
    {
        string path;
        do
        {
            path = Guid.NewGuid().ToString();
        } while (Directory.Exists(path));

        OutputPath = path + Path.DirectorySeparatorChar;
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        // Safety checks to not delete the wrong directory.
        OutputPath.Should().HaveLength(37);
        OutputPath[..36].Should().NotContain(Path.DirectorySeparatorChar.ToString());

        if (Directory.Exists(OutputPath)) Directory.Delete(OutputPath, true);
    }
}