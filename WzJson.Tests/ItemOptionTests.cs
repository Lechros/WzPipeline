using Newtonsoft.Json;

namespace WzJson.Tests;

[TestClass]
public class ItemOptionTests
{
    [TestMethod]
    public void JsonSerialize_WithAllProperties_EqualsExpectedJson()
    {
        const string expectedJson =
            @"{""optionType"":10,""reqLevel"":200,""string"":""test string"",""level"":{""2"":{""test-prop 1"":1},""10"":{""test-prop 2"":6},""24"":{""test-prop 3"":20,""test-prop 4"":3}}}";

        var itemOption = new ItemOption.ItemOption
        {
            OptionType = 10,
            ReqLevel = 200,
            String = "test string",
            Level = new SortedDictionary<int, Dictionary<string, object>>
            {
                [2] = new() { ["test-prop 1"] = 1 },
                [10] = new() { ["test-prop 2"] = 6 },
                [24] = new() { ["test-prop 3"] = 20, ["test-prop 4"] = 3 }
            }
        };

        var json = JsonConvert.SerializeObject(itemOption);

        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    public void JsonSerialize_WithNoProperties_EqualsExpectedJson()
    {
        const string expectedJson = @"{}";

        var itemOption = new ItemOption.ItemOption();

        var json = JsonConvert.SerializeObject(itemOption);

        Assert.AreEqual(expectedJson, json);
    }
}