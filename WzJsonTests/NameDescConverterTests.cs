using WzJson;

namespace WzJsonTests;

[TestClass]
public class NameDescConverterTests
{
    private const string MaplePath = @"C:\Nexon\Maple";
    private IWzProvider wzProvider;

    [TestInitialize]
    public void SetupWzProvider()
    {
        wzProvider = new WzProvider(MaplePath);
    }

    [TestMethod]
    [DataRow("1000000", "파란색 털모자", null)]
    [DataRow("1002702", "레전드 두건", "신규월드 '레전드'의 최초 정착자에게 주는 선물이다.")]
    public void Convert_StringNode_EqualsExpected(string code, string expectedName, string expectedDesc)
    {
        var path = @$"String\Eqp.img\Eqp\Cap\{code}";
        var node = wzProvider.BaseNode.FindNodeByPath(path, true);
        var converter = new NameDescConverter();
        
        Assert.IsNotNull(node);

        var (name, desc) = converter.ConvertNode(node);
        
        Assert.AreEqual(expectedName, name);
        Assert.AreEqual(expectedDesc, desc);
    }
}