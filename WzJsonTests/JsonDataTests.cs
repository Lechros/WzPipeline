using WzJson;

namespace WzJsonTests;

[TestClass]
public class JsonDataTests
{
    [TestMethod]
    public void Ctor_Name_PropertiesReturn()
    {
        const string Name = "test-data.json";

        var data = new JsonData(Name);

        Assert.AreEqual(Name, data.Path);
    }

    [TestMethod]
    public void Ctor_NameAndDictionary_PropertiesReturn()
    {
        const string Name = "test-data.json";
        const string Key = "1234567";
        const int Value = 123;

        var data = new JsonData(Name, new Dictionary<string, object>
        {
            [Key] = 123
        });

        Assert.AreEqual(Name, data.Path);
        Assert.AreEqual(1, data.Items.Count);
        Assert.AreEqual(Value, data.Items[Key]);
    }
}