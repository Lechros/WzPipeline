namespace WzJson.Common.Tests;

[TestClass]
public class NameDescTests
{
    [TestMethod]
    [DataRow(null, null)]
    [DataRow("name", null)]
    [DataRow(null, "desc")]
    [DataRow("name", "desc")]
    public void Ctor_NameAndDesc_PropertiesAreEqual(string name, string desc)
    {
        var nameDesc = new NameDesc(name, desc);

        Assert.AreEqual(name, nameDesc.Name);
        Assert.AreEqual(desc, nameDesc.Desc);
    }
}