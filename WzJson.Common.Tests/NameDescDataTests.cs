using WzJson.Common.Data;

namespace WzJson.Common.Tests;

[TestClass]
public class NameDescDataTests
{
    [TestMethod]
    public void Ctor_Empty_PropertiesReturn()
    {
        var data = new NameDescData();

        Assert.IsInstanceOfType(data.Items, typeof(IDictionary<string, NameDesc>));
    }

    [TestMethod]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string key = "1234567";
        var value = new NameDesc("name", "desc");
        var data = new NameDescData();

        data.Add(key, value);

        Assert.AreEqual(value, data.Items[key]);
    }
}