namespace WzJson.Common.Tests;

public class NameDescTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("name", null)]
    [InlineData(null, "desc")]
    [InlineData("name", "desc")]
    public void Ctor_NameAndDesc_PropertiesAreEqual(string? name, string? desc)
    {
        var nameDesc = new NameDesc(name, desc);

        Assert.Equal(name, nameDesc.Name);
        Assert.Equal(desc, nameDesc.Desc);
    }
}