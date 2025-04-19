using FluentAssertions;
using WzComparerR2.WzLib;

namespace WzJson.Common.Tests;

[TestFixture]
public class AbstractNodeConverterTests
{
    private const string TestKey = "test-key";
    private const string TestItem = "test-key-add";

    private AbstractNodeConverter<string> converter;
    private Wz_Node node;

    [OneTimeSetUp]
    public void SetUp()
    {
        converter = new TestNodeConverter();
        node = new Wz_Node();
    }

    [Test]
    public void GetNodeKey_EmptyWzNode_ShouldReturnTestKey()
    {
        var nodeKey = converter.GetNodeKey(node);

        nodeKey.Should().Be(TestKey);
    }

    [Test]
    public void Convert_WzNode_ShouldReturnTestItem()
    {
        var item = converter.Convert(node, TestKey);

        item.Should().Be(TestItem);
    }

    [Test]
    public void Convert_AsINodeConverter_ShouldReturnTestItem()
    {
        var item = ((INodeConverter)converter).Convert(node, TestKey);

        item.Should().Be(TestItem);
    }

    private class TestNodeConverter : AbstractNodeConverter<string>
    {
        public override string GetNodeKey(Wz_Node node)
        {
            return TestKey;
        }

        public override string? Convert(Wz_Node node, string key)
        {
            return key + "-add";
        }
    }
}