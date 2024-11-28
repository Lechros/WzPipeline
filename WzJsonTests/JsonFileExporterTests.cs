using Newtonsoft.Json;
using WzJson;

namespace WzJsonTests;

[TestClass]
public class JsonFileExporterTests : OutputPathTestSupport
{
    private JsonSerializer jsonSerializer = new();

    [TestMethod]
    public void Ctor_FileOutputPath_ThrowsArgumentException()
    {
        string path = Path.Join(OutputPath, "test.json");

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
        const string Filename = "test.json";
        const string Key = "key";
        const string Value = "value";
        string expectedFilename = Path.Join(OutputPath, Filename);
        string expectedContent = @"{""key"":""value""}";

        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData(Filename, new Dictionary<string, object> { [Key] = Value });
        exporter.Export(data);

        Assert.IsTrue(File.Exists(expectedFilename));
        var content = File.ReadAllText(expectedFilename);
        Assert.AreEqual(expectedContent, content);
    }

    [TestMethod]
    public void Export_NestedPathName_SaveSuccesses()
    {
        const string Filename = "nested/path/test.json";
        const string Key = "key";
        const string Value = "value";
        string expectedFilename = Path.Join(OutputPath, Filename);
        string expectedContent = @"{""key"":""value""}";

        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData(Filename, new Dictionary<string, object> { [Key] = Value });

        exporter.Export(data);

        Assert.IsTrue(File.Exists(expectedFilename));
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
        
        public void Add<T>(string name, T item)
        {
            throw new NotImplementedException();
        }
    }
}