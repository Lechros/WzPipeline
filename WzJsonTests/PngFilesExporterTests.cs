using System.Drawing;
using WzJson;

namespace WzJsonTests;

[TestClass]
public class PngFilesExporterTests : OutputPathTestSupport
{
    [TestMethod]
    public void Ctor_FileOutputPath_ThrowsArgumentException()
    {
        string path = Path.Join(OutputPath, "test.png");

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
        const string Name = "test-images";
        const string Key1 = "test1.png";
        const string Key2 = "test2.png";
        using Bitmap Value1 = new(1, 1);
        using Bitmap Value2 = new(1, 1);
        string expectedOutputDirectory = Path.Join(OutputPath, Name);
        string expectedFile1 = Path.Join(expectedOutputDirectory, Key1);
        string expectedFile2 = Path.Join(expectedOutputDirectory, Key2);

        var exporter = new PngFilesExporter(OutputPath);
        var data = new BitmapData(Name, new Dictionary<string, Bitmap>
        {
            [Key1] = Value1,
            [Key2] = Value2
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
        const string Name = "/nested/path/test-images";
        const string Key1 = "test1.png";
        const string Key2 = "test2.png";
        using Bitmap Value1 = new(1, 1);
        using Bitmap Value2 = new(1, 1);
        string expectedOutputDirectory = Path.Join(OutputPath, Name);
        string expectedFile1 = Path.Join(expectedOutputDirectory, Key1);
        string expectedFile2 = Path.Join(expectedOutputDirectory, Key2);

        var exporter = new PngFilesExporter(OutputPath);
        var data = new BitmapData(Name, new Dictionary<string, Bitmap>
        {
            [Key1] = Value1,
            [Key2] = Value2
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
        const string Name = "test-images";
        const string Key1 = "/nested/path/test1.png";
        const string Key2 = "/another/nested/path/test2.png";
        using Bitmap Value1 = new(1, 1);
        using Bitmap Value2 = new(1, 1);
        string expectedOutputDirectory = Path.Join(OutputPath, Name);
        string expectedFile1 = Path.Join(expectedOutputDirectory, Key1);
        string expectedFile2 = Path.Join(expectedOutputDirectory, Key2);

        var exporter = new PngFilesExporter(OutputPath);
        var data = new BitmapData(Name, new Dictionary<string, Bitmap>
        {
            [Key1] = Value1,
            [Key2] = Value2
        });

        exporter.Export(data);

        Assert.IsTrue(Directory.Exists(expectedOutputDirectory));
        Assert.IsTrue(File.Exists(expectedFile1));
        Assert.IsTrue(File.Exists(expectedFile2));
    }

    private class NonBitmapData : IData<Bitmap>
    {
        public NonBitmapData(string name)
        {
            Name = name;
            Items = new Dictionary<string, Bitmap>();
        }

        public string Name { get; }
        public IDictionary<string, Bitmap> Items { get; }
    }
}