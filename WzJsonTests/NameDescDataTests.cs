using WzJson;

namespace WzJsonTests;

[TestClass]
public class NameDescDataTests
{
    [TestMethod]
    public void Ctor_Empty_PropertiesReturn()
    {
        var data = new NameDescData();

        Assert.IsInstanceOfType(data.Items, typeof(IDictionary<string, NameDesc>));
    }
}