using Newtonsoft.Json;
using WzJson.Model;

namespace WzJson.Tests;

public class ItemOptionTests
{
    [Fact]
    public void JsonSerialize_WithAllProperties_EqualsExpectedJson()
    {
        const string expectedJson =
            """{"optionType":10,"reqLevel":200,"level":{"2":{"string":"STR : +1","option":{"str":1}},"10":{"string":"STR : +6","option":{"str":6}},"24":{"string":"STR : +12","option":{"str":12}}}}""";

        var itemOption = new ItemOption
        {
            OptionType = 10,
            ReqLevel = 200,
            Level = new SortedDictionary<int, ItemOption.LevelInfo>
            {
                [2] = new() { String = "STR : +1", Option = new GearOption { Str = 1 } },
                [10] = new() { String = "STR : +6", Option = new GearOption { Str = 6 } },
                [24] = new() { String = "STR : +12", Option = new GearOption { Str = 12 } }
            }
        };

        var json = JsonConvert.SerializeObject(itemOption);

        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void JsonSerialize_WithNoProperties_EqualsExpectedJson()
    {
        const string expectedJson = """{"level":{}}""";

        var itemOption = new ItemOption();

        var json = JsonConvert.SerializeObject(itemOption);

        Assert.Equal(expectedJson, json);
    }
}