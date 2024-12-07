using WzComparerR2.WzLib;

namespace WzJson.Common.Tests;

public class WzLibLearningTests
{
    private const string MaplePath = @"C:\Nexon\Maple\Data\Base\Base.wz";
    private readonly IWzProvider wzProvider;

    public WzLibLearningTests()
    {
        wzProvider = new WzProvider(MaplePath);
    }

    [Fact]
    public void FindNodeByPath_StringEqpNodeWithExtractParamTrue_ContainsChildNodes()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";

        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        Assert.True(stringEqpNode.Nodes.Count > 0);
    }

    [Fact]
    public void GetNodeWzImage_NotWzImageChildNode_ReturnsNull()
    {
        const string stringEqpNodePath = "String";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        Assert.Null(nodeWzImage);
    }

    [Fact]
    public void GetNodeWzImage_WzImageChildNode_DoesNotThrow()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();
    }

    [Fact]
    public void GetNodeWzImage_WzImageChildNode_ReturnsFirstParentImgNode()
    {
        const string stringEqpImgPath = @"String\Eqp.img";
        var stringEqpImgNode = wzProvider.BaseNode.FindNodeByPath(stringEqpImgPath, true);
        var expected = stringEqpImgNode.GetValue<Wz_Image>();
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        Assert.Equal(expected, nodeWzImage);
    }

    [Fact]
    public void Unextract_WzImageOfNonImageNode_DoesNotThrow()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);
        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        nodeWzImage.Unextract();
    }

    [Fact]
    public void GetValueAsInt_Int32Value_ReturnsIntValue()
    {
        const string path = @"Character\Weapon\01702565.img\info\cash";
        var node = wzProvider.BaseNode.FindNodeByPath(path, true);

        var value = node.GetValue<int>();

        Assert.IsType<int>(value);
        Assert.Equal(1, value);
    }
}