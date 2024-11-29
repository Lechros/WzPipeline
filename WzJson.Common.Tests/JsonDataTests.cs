using WzJson.Common.Data;

namespace WzJson.Common.Tests;

[TestClass]
public class JsonDataTests
{
    [TestMethod]
    public void Ctor_Name_PropertiesReturn()
    {
        const string path = "test-data.json";

        var data = new JsonData(path);

        Assert.AreEqual(path, data.Path);
    }

    [TestMethod]
    public void Ctor_NameAndDictionary_PropertiesReturn()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        const int value = 123;

        var data = new JsonData(path, new Dictionary<string, object>
        {
            [key] = 123
        });

        Assert.AreEqual(path, data.Path);
        Assert.AreEqual(1, data.Items.Count);
        Assert.AreEqual(value, data.Items[key]);
    }

    [TestMethod]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        const int value = 123;
        var data = new JsonData(path);

        data.Add(key, value);

        Assert.AreEqual(value, data.Items[key]);
    }
}