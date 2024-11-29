using WzComparerR2.WzLib;

namespace WzJson.Common.Tests;

[TestClass]
public class WzLibLearningTests
{
    private const string MaplePath = @"C:\Nexon\Maple";
    private IWzProvider wzProvider;

    [TestInitialize]
    public void SetupWzProvider()
    {
        wzProvider = new WzProvider(MaplePath);
    }

    [TestMethod]
    public void FindNodeByPath_StringEqpNodeWithExtractParamTrue_ContainsChildNodes()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";

        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        Assert.IsTrue(stringEqpNode.Nodes.Count > 0);
    }

    [TestMethod]
    public void GetNodeWzImage_NotWzImageChildNode_ReturnsNull()
    {
        const string stringEqpNodePath = "String";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        Assert.IsNull(nodeWzImage);
    }

    [TestMethod]
    public void GetNodeWzImage_WzImageChildNode_DoesNotThrow()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();
    }

    [TestMethod]
    public void GetNodeWzImage_WzImageChildNode_ReturnsFirstParentImgNode()
    {
        const string stringEqpImgPath = @"String\Eqp.img";
        var stringEqpImgNode = wzProvider.BaseNode.FindNodeByPath(stringEqpImgPath, true);
        var expected = stringEqpImgNode.GetValue<Wz_Image>();
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);

        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        Assert.AreEqual(expected, nodeWzImage);
    }

    [TestMethod]
    public void Unextract_WzImageOfNonImageNode_DoesNotThrow()
    {
        const string stringEqpNodePath = @"String\Eqp.img\Eqp";
        var stringEqpNode = wzProvider.BaseNode.FindNodeByPath(stringEqpNodePath, true);
        var nodeWzImage = stringEqpNode.GetNodeWzImage();

        nodeWzImage.Unextract();
    }

    [TestMethod]
    public void GetValueAsInt_Int32Value_ReturnsIntValue()
    {
        const string path = @"Character\Weapon\01702565.img\info\cash";
        var node = wzProvider.BaseNode.FindNodeByPath(path, true);

        var value = node.GetValue<int>();

        Assert.IsInstanceOfType(value, typeof(int));
        Assert.AreEqual(1, value);
    }
}