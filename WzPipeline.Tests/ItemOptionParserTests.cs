using System.Globalization;
using Wz;
using WzPipeline.Domains.Shared.ItemOption;

namespace WzPipeline.Tests;

public class ItemOptionParserTests
{
    [TestCase("incSTRr", "STR +#incSTRr%", "StrRate", 0.5)]
    [TestCase("incCriticaldamageF", "Critical damage +#incCriticaldamageF%", "CriticalDamage", 7.5)]
    [TestCase("incSTR", "STR +#incSTR", "Str", 1.0)]
    [SetCulture("fr-FR")]
    public void Parse_PreservesNumericOptionsAcrossCultures(string property, string template, string option, double value)
    {
        var node = new TestNode("060090",
            new TestNode("info", new TestNode("string") { Text = template }),
            new TestNode("level", new TestNode("1", new TestNode(property) { Number = value })));

        var result = new ItemOptionParser().Parse(new ItemOptionNode(node));

        Convert.ToDouble(result.Level[1].Option[option]).Should().Be(value);
        result.Level[1].String.Should().Be(template.Replace("#" + property, value.ToString(CultureInfo.InvariantCulture)));
    }

    [Test]
    public void Parse_PreservesFractionalBossDamage()
    {
        var node = new TestNode("035601",
            new TestNode("info", new TestNode("string") { Text = "Boss +#incDAMr%" }),
            new TestNode("level", new TestNode("1",
                new TestNode("boss") { Number = 1 }, new TestNode("incDAMr") { Number = 7.5 })));

        var result = new ItemOptionParser().Parse(new ItemOptionNode(node));

        Convert.ToDouble(result.Level[1].Option.BossDamage).Should().Be(7.5);
        Convert.ToDouble(result.Level[1].Option.Damage).Should().Be(0);
    }

    private sealed class TestNode(string name, params IWzNode[] children) : IWzNode
    {
        public string Name => name;
        public string? Text { get; init; }
        public double? Number { get; init; }
        public WzNodeType Type => Text != null ? WzNodeType.String : Number.HasValue ? WzNodeType.Single : WzNodeType.Property;
        public IWzNode? Parent => null;
        public IWzNodeCollection<IWzNode> Nodes { get; } = new TestNodeCollection(children);
        public string? GetString() => Text;
        public float GetSingle() => (float)Number!.Value;
    }

    private sealed class TestNodeCollection(IEnumerable<IWzNode> nodes) : List<IWzNode>(nodes), IWzNodeCollection<IWzNode>
    {
        public IWzNode this[string name] => this.First(node => node.Name == name);
        public IWzNode? Find(string name) => this.FirstOrDefault(node => node.Name == name);
    }
}
