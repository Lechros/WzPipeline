using System.Drawing;

namespace WzJson.Tests;

[TestClass]
public class PngFilesExporterTests : OutputPathTestSupport
{
    [TestMethod]
    public void Ctor_FileOutputPath_ThrowsArgumentException()
    {
        var path = Path.Join(OutputPath, "test.png");

        Assert.ThrowsException<ArgumentException>(() => new PngFilesExporter(path));
    }

    [TestMethod]
    public void Ctor_DirectoryOutputPath_DoesNotThrow()
    {
        var exporter = new PngFilesExporter(OutputPath);
    }

    [TestMethod]
    public void Supports_BitmapData_ReturnsTrue()
    {
        var exporter = new PngFilesExporter(OutputPath);
        var data = new BitmapData("test-images", new Dictionary<string, Bitmap>());

        Assert.IsTrue(exporter.Supports(data));
    }

    [TestMethod]
    public void Supports_NonBitmapData_ReturnsFalse()
    {
        var exporter = new PngFilesExporter(OutputPath);
        var data = new NonBitmapData("test-images");

        Assert.IsFalse(exporter.Supports(data));
    }

    [TestMethod]
    public void Export_BitmapData_SavesPngFilesInsideNameFolder()
    {
        const string filename = "test-images";
        const string key1 = "test1.png";
        const string key2 = "test2.png";
        using var value1 = new Bitmap(1, 1);
        using var value2 = new Bitmap(1, 1);
        var expectedOutputDirectory = Path.Join(OutputPath, filename);
        var expectedFile1 = Path.Join(expectedOutputDirectory, key1);
        var expectedFile2 = Path.Join(expectedOutputDirectory, key2);

        var exporter = new PngFilesExporter(OutputPath);
        var data = new BitmapData(filename, new Dictionary<string, Bitmap>
        {
            [key1] = value1,
            [key2] = value2
        });

        exporter.Export(data);

        Assert.IsTrue(Directory.Exists(expectedOutputDirectory));
        Assert.AreEqual(2, Directory.EnumerateFiles(expectedOutputDirectory).Count());
        Assert.IsTrue(File.Exists(expectedFile1));
        Assert.IsTrue(File.Exists(expectedFile2));
    }

    [TestMethod]
    public void Export_NestedPathName_SaveSuccesses()
    {
        const string filename = "/nested/path/test-images";
        const string key1 = "test1.png";
        const string key2 = "test2.png";
        using var value1 = new Bitmap(1, 1);
        using var value2 = new Bitmap(1, 1);
        var expectedOutputDirectory = Path.Join(OutputPath, filename);
        var expectedFile1 = Path.Join(expectedOutputDirectory, key1);
        var expectedFile2 = Path.Join(expectedOutputDirectory, key2);

        var exporter = new PngFilesExporter(OutputPath);
        var data = new BitmapData(filename, new Dictionary<string, Bitmap>
        {
            [key1] = value1,
            [key2] = value2
        });

        exporter.Export(data);

        Assert.IsTrue(Directory.Exists(expectedOutputDirectory));
        Assert.AreEqual(2, Directory.EnumerateFiles(expectedOutputDirectory).Count());
        Assert.IsTrue(File.Exists(expectedFile1));
        Assert.IsTrue(File.Exists(expectedFile2));
    }

    [TestMethod]
    public void Export_NestedPathKey_SaveSuccesses()
    {
        const string filename = "test-images";
        const string key1 = "/nested/path/test1.png";
        const string key2 = "/another/nested/path/test2.png";
        using var value1 = new Bitmap(1, 1);
        using var value2 = new Bitmap(1, 1);
        var expectedOutputDirectory = Path.Join(OutputPath, filename);
        var expectedFile1 = Path.Join(expectedOutputDirectory, key1);
        var expectedFile2 = Path.Join(expectedOutputDirectory, key2);

        var exporter = new PngFilesExporter(OutputPath);
        var data = new BitmapData(filename, new Dictionary<string, Bitmap>
        {
            [key1] = value1,
            [key2] = value2
        });

        exporter.Export(data);

        Assert.IsTrue(Directory.Exists(expectedOutputDirectory));
        Assert.IsTrue(File.Exists(expectedFile1));
        Assert.IsTrue(File.Exists(expectedFile2));
    }

    private class NonBitmapData : IData
    {
        public NonBitmapData(string path)
        {
            Path = path;
            Items = new Dictionary<string, Bitmap>();
        }

        public string Path { get; }
        public IDictionary<string, Bitmap> Items { get; }

        public void Add<T>(string name, T item) where T : notnull
        {
            throw new NotImplementedException();
        }
    }
}