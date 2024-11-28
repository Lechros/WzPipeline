using WzJson;

namespace WzJsonTests;

[TestClass]
public class JsonDataTests
{
    [TestMethod]
    public void Ctor_Name_PropertiesReturn()
    {
        const string Path = "test-data.json";

        var data = new JsonData(Path);

        Assert.AreEqual(Path, data.Path);
    }

    [TestMethod]
    public void Ctor_NameAndDictionary_PropertiesReturn()
    {
        const string Path = "test-data.json";
        const string Key = "1234567";
        const int Value = 123;

        var data = new JsonData(Path, new Dictionary<string, object>
        {
            [Key] = 123
        });

        Assert.AreEqual(Path, data.Path);
        Assert.AreEqual(1, data.Items.Count);
        Assert.AreEqual(Value, data.Items[Key]);
    }

    [TestMethod]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string Path = "test-data.json";
        const string Key = "1234567";
        const int Value = 123;
        var data = new JsonData(Path);

        data.Add(Key, Value);

        Assert.AreEqual(Value, data.Items[Key]);
    }
}