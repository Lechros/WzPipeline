using FluentAssertions;
using WzJson.Common.Converter;

namespace WzJson.Common.Tests;

[TestFixture]
public class WzStringConverterTests
{
    private const string MaplePath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    private readonly IWzProvider wzProvider;

    public WzStringConverterTests()
    {
        wzProvider = new WzProvider(MaplePath);
    }
    
    [TestCase("1000000", "파란색 털모자", null)]
    [TestCase("1002702", "레전드 두건", "신규월드 '레전드'의 최초 정착자에게 주는 선물이다.")]
    public void Convert_StringNode_EqualsExpected(string code, string expectedName, string expectedDesc)
    {
        var path = @$"String\Eqp.img\Eqp\Cap\{code}";
        var node = wzProvider.BaseNode.FindNodeByPath(path, true);
        var converter = new WzStringConverter();

        node.Should().NotBeNull();

        var wzString = converter.Convert(node, string.Empty);

        wzString.Name.Should().Be(expectedName);
        wzString.Desc.Should().Be(expectedDesc);
    }
}