using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wz;
using WzPipeline.Application.Shared.Json;
using WzPipeline.Domains.Gear;
using WzPipeline.Domains.Shared.String;

namespace WzPipeline.Tests;

public class GearParserTests
{
    private readonly GearParser parser = new();
    private readonly GearParseContext context = new()
    {
        GearStringData = new Dictionary<string, NameDesc> { ["1000000"] = new("Test gear", null) },
        ItemOptionData = new(),
        SkillNameData = new Dictionary<string, string>(),
        AstraSubWeaponData = new()
    };

    [TestCase(null)]
    [TestCase(0)]
    [TestCase(10)]
    public void Parse_PreservesCuttableCountPresence(int? count)
    {
        var info = new TestNode("info");
        if (count.HasValue) info.Children.Add(new TestNode("CuttableCount", count.Value));
        var root = new TestNode("01000000.img");
        root.Children.Add(info);

        var gear = parser.Parse(new GearNode(root), context).Single();

        gear.Attributes.CuttableCount.Should().Be(count);
        gear.Attributes.TotalCuttableCount.Should().Be(count);
        using var services = new ServiceCollection().AddApplicationJson().BuildServiceProvider();
        var json = JObject.FromObject(gear, services.GetRequiredService<JsonSerializer>());
        var attributes = (JObject)json["attributes"]!;
        if (count.HasValue)
        {
            attributes["cuttableCount"]!.Value<int>().Should().Be(count.Value);
            attributes["totalCuttableCount"]!.Value<int>().Should().Be(count.Value);
        }
        else
        {
            attributes.ContainsKey("cuttableCount").Should().BeFalse();
            attributes.ContainsKey("totalCuttableCount").Should().BeFalse();
        }
    }

    [TestCase(null, 0)]
    [TestCase(0, -1)]
    [TestCase(1, 0)]
    [TestCase(2, 1)]
    [TestCase(3, 2)]
    [TestCase(5, 3)]
    [TestCase(7, 4)]
    public void Parse_SetsPotentialGradeOnlyWhenFixedGradeExists(int? fixedGrade, int expected)
    {
        var info = new TestNode("info");
        if (fixedGrade.HasValue) info.Children.Add(new TestNode("fixedGrade", fixedGrade.Value));
        var root = new TestNode("01000000.img");
        root.Children.Add(info);

        var gear = parser.Parse(new GearNode(root), context).Single();

        gear.PotentialGrade.Should().Be(expected);
        using var services = new ServiceCollection().AddApplicationJson().BuildServiceProvider();
        var json = JObject.FromObject(gear, services.GetRequiredService<JsonSerializer>());
        if (expected == 0)
            json.ContainsKey("potentialGrade").Should().BeFalse();
        else
            json["potentialGrade"]!.Value<int>().Should().Be(expected);
    }

    private sealed class TestNode(string name, int? value = null) : IWzNode
    {
        public string Name => name;
        public WzNodeType Type => value.HasValue ? WzNodeType.Int32 : WzNodeType.Property;
        public IWzNode? Parent => null;
        public TestNodeCollection Children { get; } = new();
        public IWzNodeCollection<IWzNode> Nodes => Children;
        public int GetInt32() => value!.Value;
    }

    private sealed class TestNodeCollection : List<IWzNode>, IWzNodeCollection<IWzNode>
    {
        public IWzNode this[string name] => this.First(node => node.Name == name);
        public IWzNode? Find(string name) => this.FirstOrDefault(node => node.Name == name);
    }
}
