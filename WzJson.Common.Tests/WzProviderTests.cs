using WzComparerR2.WzLib;

namespace WzJson.Common.Tests;

public class WzProviderTests
{
    private const string MaplePath = @"C:\Nexon\Maple";
    private const string StringNodePath = "String";
    private const string StringEqpNodePath = @"String\Eqp.img";

    [Fact]
    public void Ctor_MaplePath_DoesNotThrow()
    {
        var provider = new WzProvider(MaplePath);
    }

    [Fact]
    public void BaseNode__ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);

        var baseNode = provider.BaseNode;

        Assert.IsType<Wz_Node>(baseNode);
    }

    [Fact]
    public void FindNodeByPath_StringNodeFromBaseNode_ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringNode = baseNode.FindNodeByPath(StringNodePath);

        Assert.IsType<Wz_Node>(stringNode);
    }

    [Fact]
    public void FindNodeByPath_StringEqpImgFromBaseNode_ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringEqpImg = baseNode.FindNodeByPath(StringEqpNodePath);

        Assert.IsType<Wz_Node>(stringEqpImg);
    }

    [Fact]
    public void FindNodeByPath_StringEqpImgFromBaseNode_ReturnsWzNodeWithWzImage()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringEqpNode = baseNode.FindNodeByPath(StringEqpNodePath);
        var stringEqpNodeWzImage = stringEqpNode.GetNodeWzImage();

        Assert.IsType<Wz_Image>(stringEqpNodeWzImage);
    }
}