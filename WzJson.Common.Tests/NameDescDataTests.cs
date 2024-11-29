using WzJson.Common.Data;

namespace WzJson.Common.Tests;

public class NameDescDataTests
{
    [Fact]
    public void Ctor_Empty_PropertiesReturn()
    {
        var data = new NameDescData();

        Assert.IsAssignableFrom<IDictionary<string, NameDesc>>(data.Items);
    }

    [Fact]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string key = "1234567";
        var value = new NameDesc("name", "desc");
        var data = new NameDescData();

        data.Add(key, value);

        Assert.Equal(value, data.Items[key]);
    }
}