using FluentAssertions;
using WzComparerR2.WzLib;

namespace WzJson.Common.Tests;

[TestFixture]
public class WzLibLearningTests
{
    private const string MaplePath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    private readonly IWzProvider wzProvider;

    public WzLibLearningTests()
    {
        wzProvider = new WzProvider(MaplePath);
    }

    [Test]
    public void FindNodeByPath_StringEqpNodeWithExtractParamTrue_ContainsChildNodes()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";

        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        stringEqpNode.Nodes.Should().HaveCountGreaterThan(0);
    }

    [Test]
    public void GetNodeWzImage_NotWzImageChildNode_ReturnsNull()
    {
        const string stringEqpNodePath = "String";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        nodeWzImage.Should().BeNull();
    }

    [Test]
    public void GetNodeWzImage_WzImageChildNode_DoesNotThrow()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();
    }

    [Test]
    public void GetNodeWzImage_WzImageChildNode_ReturnsFirstParentImgNode()
    {
        const string stringEqpImgPath = @"String\Eqp.img";
        var stringEqpImgNode = wzProvider.BaseNode.FindNodeByPath(stringEqpImgPath, true);
        var expected = stringEqpImgNode.GetValue<Wz_Image>();
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        nodeWzImage.Should().Be(expected);
    }

    [Test]
    public void Unextract_WzImageOfNonImageNode_DoesNotThrow()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);
        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        nodeWzImage.Unextract();
    }

    [Test]
    public void GetValueAsInt_Int32Value_ReturnsIntValue()
    {
        const string path = @"Character\Weapon\01702565.img\info\cash";
        var node = wzProvider.BaseNode.FindNodeByPath(path, true);

        var value = node.GetValue<int>();

        value.Should().BeOfType(typeof(int));
        value.Should().Be(1);
    }
}