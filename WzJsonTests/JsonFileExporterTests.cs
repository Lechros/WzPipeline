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
        var data = new JsonData<string>("test.json", new Dictionary<string, string>());

        Assert.IsTrue(exporter.Supports(data));
    }

    [TestMethod]
    public void Supports_NonJsonData_ReturnsFalse()
    {
        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new NonJsonData<string>("test.not.json");

        Assert.IsFalse(exporter.Supports(data));
    }

    [TestMethod]
    public void Export_JsonData_SavesSingleJsonFileWithName()
    {
        const string Name = "test.json";
        const string Key = "key";
        const string Value = "value";
        string expectedFilename = Path.Join(OutputPath, Name);
        string expectedContent = @"{""key"":""value""}";

        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData<string>(Name, new Dictionary<string, string> { [Key] = Value });
        exporter.Export(data);

        Assert.IsTrue(File.Exists(expectedFilename));
        var content = File.ReadAllText(expectedFilename);
        Assert.AreEqual(expectedContent, content);
    }

    [TestMethod]
    public void Export_NestedPathName_SaveSuccesses()
    {
        const string Name = "nested/path/test.json";
        const string Key = "key";
        const string Value = "value";
        string expectedFilename = Path.Join(OutputPath, Name);
        string expectedContent = @"{""key"":""value""}";

        var exporter = new JsonFileExporter(OutputPath, jsonSerializer);
        var data = new JsonData<string>(Name, new Dictionary<string, string> { [Key] = Value });

        exporter.Export(data);

        Assert.IsTrue(File.Exists(expectedFilename));
        var content = File.ReadAllText(expectedFilename);
        Assert.AreEqual(expectedContent, content);
    }

    private class NonJsonData<T> : IData<T>
    {
        public NonJsonData(string name)
        {
            Name = name;
            Items = new Dictionary<string, T>();
        }

        public string Name { get; }
        public IDictionary<string, T> Items { get; }
    }
}