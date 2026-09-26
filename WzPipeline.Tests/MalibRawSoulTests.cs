using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Soul;

namespace WzPipeline.Tests;

public class MalibRawSoulTests
{
    [Test]
    public void Serialize_WritesOptionsAsArray()
    {
        var soul = new MalibRawSoul
        {
            Id = 2591085,
            Name = "First soul",
            Magnificent = true,
            Options = [new GearOption { AttackPower = 5 }, new GearOption { MagicPower = 7 }]
        };

        var json = JObject.Parse(JsonConvert.SerializeObject(soul));

        json[nameof(MalibRawSoul.Options)].Should().BeOfType<JArray>();
        var options = (JArray)json[nameof(MalibRawSoul.Options)]!;
        options.Should().HaveCount(2);
        options[0][nameof(GearOption.AttackPower)]!.Value<int>().Should().Be(5);
        options[1][nameof(GearOption.MagicPower)]!.Value<int>().Should().Be(7);
    }
}