using FluentAssertions;
using WzComparerR2.WzLib;

namespace WzJson.Common.Tests;

[TestFixture]
public class WzProviderTests
{
    private const string MaplePath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    private const string StringNodePath = "String";
    private const string StringEqpNodePath = @"String\Eqp.img";

    [Test]
    public void Ctor_MaplePath_DoesNotThrow()
    {
        var provider = new WzProvider(MaplePath);
    }

    [Test]
    public void BaseNode__ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);

        var baseNode = provider.BaseNode;

        baseNode.Should().BeOfType<Wz_Node>();
    }

    [Test]
    public void FindNodeByPath_StringNodeFromBaseNode_ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringNode = baseNode.FindNodeByPath(StringNodePath);

        stringNode.Should().BeOfType<Wz_Node>();
    }

    [Test]
    public void FindNodeByPath_StringEqpImgFromBaseNode_ReturnsWzNode()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringEqpImg = baseNode.FindNodeByPath(StringEqpNodePath);

        stringEqpImg.Should().BeOfType<Wz_Node>();
    }

    [Test]
    public void FindNodeByPath_StringEqpImgFromBaseNode_ReturnsWzNodeWithWzImage()
    {
        var provider = new WzProvider(MaplePath);
        var baseNode = provider.BaseNode;

        var stringEqpNode = baseNode.FindNodeByPath(StringEqpNodePath);
        var stringEqpNodeWzImage = stringEqpNode.GetNodeWzImage();

        stringEqpNodeWzImage.Should().BeOfType<Wz_Image>();
    }
}