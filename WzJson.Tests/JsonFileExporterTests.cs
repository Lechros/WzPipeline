using Newtonsoft.Json;

namespace WzJson.Tests;

[TestClass]
public class JsonFileExporterTests : OutputPathTestSupport
{
    private readonly JsonSerializer jsonSerializer = new();

    [TestMethod]
    public void Ctor_FileOutputPath_ThrowsArgumentException()
    {
        var path = Path.Join(OutputPath, "test.json");

        Assert.ThrowsException<ArgumentException>(() => new JsonFileExporter(path, jsonSerializer));
    }

    [TestMethod]
    public void Ctor_DirectoryOutputPath_DoesNotThrow()
    {
        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
    }

    [TestMethod]
    public void Supports_JsonData_ReturnsTrue()
    {
        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData("test.json", new Dictionary<string, object>());

        Assert.IsTrue(exporter.Supports(data));
    }

    [TestMethod]
    public void Supports_NonJsonData_ReturnsFalse()
    {
        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new NonJsonData("test.not.json");

        Assert.IsFalse(exporter.Supports(data));
    }

    [TestMethod]
    public void Export_JsonData_SavesSingleJsonFileWithName()
    {
        const string filename = "test.json";
        const string key = "key";
        const string value = "value";
        var expectedFilename = Path.Join(OutputPath, filename);
        var expectedContent = @"{""key"":""value""}";

        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData(filename, new Dictionary<string, object> { [key] = value });
        exporter.Export(data);

        Assert.IsTrue(File.Exists(expectedFilename));
        var content = File.ReadAllText(expectedFilename);
        Assert.AreEqual(expectedContent, content);
    }

    [TestMethod]
    public void Export_NestedPathName_SaveSuccesses()
    {
        const string filename = "nested/path/test.json";
        const string key = "key";
        const string value = "value";
        var expectedFilename = Path.Join(OutputPath, filename);
        var expectedContent = @"{""key"":""value""}";

        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData(filename, new Dictionary<string, object> { [key] = value });

        exporter.Export(data);

        Assert.IsTrue(File.Exists(expectedFilename));
        var content = File.ReadAllText(expectedFilename);
        Assert.AreEqual(expectedContent, content);
    }

    [TestMethod]
    public void Export_NumberKeys_SortedInNaturalOrder()
    {
        const string filename = "test.json";
        var expectedFilename = Path.Join(OutputPath, filename);
        var dict = new Dictionary<string, object>
        {
            ["1"] = "1",
            ["11"] = "11",
            ["2"] = "2",
            ["21"] = "21",
            ["10"] = "10",
            ["-1"] = "-1"
        };
        var expectedContent =
            @"{""-1"":""-1"",""1"":""1"",""2"":""2"",""10"":""10"",""11"":""11"",""21"":""21""}";

        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData(filename, dict);

        exporter.Export(data);

        var content = File.ReadAllText(expectedFilename);
        Assert.AreEqual(expectedContent, content);
    }

    private class NonJsonData : IData
    {
        public NonJsonData(string path)
        {
            Path = path;
            Items = new Dictionary<string, object>();
        }

        public string Path { get; }
        public IDictionary<string, object> Items { get; }

        public void Add<T>(string name, T item) where T : notnull
        {
            throw new NotImplementedException();
        }
    }
}