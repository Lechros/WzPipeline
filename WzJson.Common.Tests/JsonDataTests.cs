using WzJson.Common.Data;

namespace WzJson.Common.Tests;

public class JsonDataTests
{
    [Fact]
    public void Ctor_Name_PropertiesReturn()
    {
        const string path = "test-data.json";

        var data = new JsonData(path);

        Assert.Equal(path, data.Path);
    }

    [Fact]
    public void Ctor_NameAndDictionary_PropertiesReturn()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        const int value = 123;

        var data = new JsonData(path, new Dictionary<string, object>
        {
            [key] = 123
        });

        Assert.Equal(path, data.Path);
        Assert.Equal(1, data.Items.Count);
        Assert.Equal(value, data.Items[key]);
    }

    [Fact]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        const int value = 123;
        var data = new JsonData(path);

        data.Add(key, value);

        Assert.Equal(value, data.Items[key]);
    }
}