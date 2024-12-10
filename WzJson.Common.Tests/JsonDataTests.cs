using WzJson.Common.Data;

namespace WzJson.Common.Tests;

public class JsonDataTests
{
    [Fact]
    public void Ctor_LabelAndPath_PropertiesReturn()
    {
        const string label = "label";
        const string path = "test-data.json";

        var data = new JsonData<object>(label, path);

        Assert.Equal(label, data.Label);
        Assert.Equal(path, data.Path);
    }

    [Fact]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string label = "label";
        const string path = "test-data.json";
        const string key = "1234567";
        const int value = 123;
        var data = new JsonData<int>(label, path);

        data.Add(key, value);

        Assert.Equal(value, data[key]);
    }
}