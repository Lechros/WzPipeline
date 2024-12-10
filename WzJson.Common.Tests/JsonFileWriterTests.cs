using Newtonsoft.Json;
using WzJson.Common.Data;
using WzJson.Common.Writer;

namespace WzJson.Common.Tests;

public class JsonFileWriterTests : OutputPathTestSupport
{
    private readonly JsonSerializer jsonSerializer = new();

    [Fact]
    public void Ctor_FileOutputPath_ThrowsArgumentException()
    {
        var path = Path.Join(OutputPath, "test.json");

        Assert.Throws<ArgumentException>(() => new JsonFileWriter(path, jsonSerializer));
    }

    [Fact]
    public void Ctor_DirectoryOutputPath_DoesNotThrow()
    {
        var writer = new JsonFileWriter(OutputPath, jsonSerializer);
    }

    [Fact]
    public void Supports_JsonData_ReturnsTrue()
    {
        var writer = new JsonFileWriter(OutputPath, jsonSerializer);
        var data = new JsonData<object>("", "test.json");

        Assert.True(writer.Supports(data));
    }

    [Fact]
    public void Supports_NonJsonData_ReturnsFalse()
    {
        var writer = new JsonFileWriter(OutputPath, jsonSerializer);
        var data = new NonJsonData();

        Assert.False(writer.Supports(data));
    }

    [Fact]
    public void Write_JsonData_SavesSingleJsonFileWithName()
    {
        const string filename = "test.json";
        const string key = "key";
        const string value = "value";
        var expectedFilename = Path.Join(OutputPath, filename);
        var expectedContent = @"{""key"":""value""}";
        var data = new JsonData<string>(filename, filename);
        data.Add(key, value);
        
        var writer = new JsonFileWriter(OutputPath, jsonSerializer);
        writer.Write(data, new Progress<WriteProgressData>());

        Assert.True(File.Exists(expectedFilename));
        var content = File.ReadAllText(expectedFilename);
        Assert.Equal(expectedContent, content);
    }

    [Fact]
    public void Write_NestedPathName_SaveSuccesses()
    {
        const string filename = "nested/path/test.json";
        const string key = "key";
        const string value = "value";
        var expectedFilename = Path.Join(OutputPath, filename);
        var expectedContent = @"{""key"":""value""}";
        var data = new JsonData<string>(filename, filename);
        data.Add(key, value);

        var writer = new JsonFileWriter(OutputPath, jsonSerializer);
        writer.Write(data, new Progress<WriteProgressData>());

        Assert.True(File.Exists(expectedFilename));
        var content = File.ReadAllText(expectedFilename);
        Assert.Equal(expectedContent, content);
    }

    [Fact]
    public void Write_NumberKeys_SortedInNaturalOrder()
    {
        const string filename = "test.json";
        var expectedFilename = Path.Join(OutputPath, filename);
        var expectedContent =
            @"{""-1"":""-1"",""1"":""1"",""2"":""2"",""10"":""10"",""11"":""11"",""21"":""21""}";
        var data = new JsonData<string>(filename, filename);
        data.Add("1", "1");
        data.Add("11", "11");
        data.Add("2", "2");
        data.Add("21", "21");
        data.Add("10", "10");
        data.Add("-1", "-1");

        var writer = new JsonFileWriter(OutputPath, jsonSerializer);
        writer.Write(data, new Progress<WriteProgressData>());

        var content = File.ReadAllText(expectedFilename);
        Assert.Equal(expectedContent, content);
    }

    private class NonJsonData : DefaultKeyValueData<object>;
}