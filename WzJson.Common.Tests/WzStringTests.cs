using WzJson.Common.Data;

namespace WzJson.Common.Tests;

public class WzStringTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("name", null)]
    [InlineData(null, "desc")]
    [InlineData("name", "desc")]
    public void ObjectInitializer_NameAndDesc_PropertiesAreEqual(string? name, string? desc)
    {
        var wzString = new WzString
        {
            Name = name,
            Desc = desc
        };

        Assert.Equal(name, wzString.Name);
        Assert.Equal(desc, wzString.Desc);
    }
}