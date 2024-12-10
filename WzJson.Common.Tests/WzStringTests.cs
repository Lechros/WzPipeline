using FluentAssertions;
using WzJson.Common.Data;

namespace WzJson.Common.Tests;

public class WzStringTests
{
    [TestCase(null, null)]
    [TestCase("name", null)]
    [TestCase(null, "desc")]
    [TestCase("name", "desc")]
    public void ObjectInitializer_NameAndDesc_PropertiesAreEqual(string? name, string? desc)
    {
        var wzString = new WzString
        {
            Name = name,
            Desc = desc
        };

        wzString.Name.Should().Be(name);
        wzString.Desc.Should().Be(desc);
    }
}