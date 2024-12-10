using WzJson.Common.Converter;

namespace WzJson.Common.Tests;

public class WzStringConverterTests
{
    private const string MaplePath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    private readonly IWzProvider wzProvider;

    public WzStringConverterTests()
    {
        wzProvider = new WzProvider(MaplePath);
    }
    
    [Theory]
    [InlineData("1000000", "파란색 털모자", null)]
    [InlineData("1002702", "레전드 두건", "신규월드 '레전드'의 최초 정착자에게 주는 선물이다.")]
    public void Convert_StringNode_EqualsExpected(string code, string expectedName, string expectedDesc)
    {
        var path = @$"String\Eqp.img\Eqp\Cap\{code}";
        var node = wzProvider.BaseNode.FindNodeByPath(path, true);
        var converter = new WzStringConverter();

        Assert.NotNull(node);

        var wzString = converter.Convert(node, string.Empty);

        Assert.Equal(expectedName, wzString.Name);
        Assert.Equal(expectedDesc, wzString.Desc);
    }
}