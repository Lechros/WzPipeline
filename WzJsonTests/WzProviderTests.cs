using WzComparerR2.WzLib;
using WzJson;

namespace WzJsonTests;

[TestClass]
public class WzProviderTests
{
    private const string MaplePath = @"C:\Nexon\Maple";
    private const string StringNodePath = "String";
    private const string StringEqpNodePath = @"String\Eqp.img";

    [TestMethod]
    public void Ctor_MaplePath_DoesNotThrow()
    {
        var provider = new WzProvider(MaplePath);
    }

    [TestMethod]
    public void BaseNode__ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);

        var baseNode = provider.BaseNode;

        Assert.IsInstanceOfType(baseNode, typeof(Wz_Node));
    }

    [TestMethod]
    public void FindNodeByPath_StringNodeFromBaseNode_ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringNode = baseNode.FindNodeByPath(StringNodePath);

        Assert.IsInstanceOfType(stringNode, typeof(Wz_Node));
    }

    [TestMethod]
    public void FindNodeByPath_StringEqpImgFromBaseNode_ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringEqpImg = baseNode.FindNodeByPath(StringEqpNodePath);

        Assert.IsInstanceOfType(stringEqpImg, typeof(Wz_Node));
    }

    [TestMethod]
    public void FindNodeByPath_StringEqpImgFromBaseNode_ReturnsWzNodeWithWzImage()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringEqpNode = baseNode.FindNodeByPath(StringEqpNodePath);
        var stringEqpNodeWzImage = stringEqpNode.GetNodeWzImage();

        Assert.IsInstanceOfType(stringEqpNodeWzImage, typeof(Wz_Image));
    }
}