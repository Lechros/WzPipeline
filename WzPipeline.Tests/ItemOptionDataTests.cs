using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WzPipeline.Application.Configuration;
using WzPipeline.Application.Pipelines;
using WzPipeline.Application.Shared.Json;
using WzPipeline.Domains.Shared;
using WzPipeline.Domains.Shared.ItemOption;

namespace WzPipeline.Tests;

public class ItemOptionDataTests
{
    [Test]
    public void Serialize_WritesLevelsUnderOptionIdWithoutDuplicateCode()
    {
        var data = new ItemOptionData
        {
            [1] = new() { Code = 1, Level = { [1] = new LevelOption
            {
                String = "STR +1", Option = new GearOption { Str = 1 }
            } } }
        };
        using var services = new ServiceCollection().AddApplicationJson().BuildServiceProvider();

        var json = JObject.FromObject(data, services.GetRequiredService<JsonSerializer>());

        JToken.DeepEquals(json, JObject.Parse("""
            {"1":{"level":{"1":{"string":"STR +1","option":{"str":1}}}}}
            """)).Should().BeTrue();
    }

    [Test]
    public void Select_ItemOptionDataHasExportPathAndRegisteredPipeline()
    {
        var selected = PipelineSelection.Select(["ItemOptionData"]);

        selected.Should().Equal(PipelineIds.ItemOptionData);
        PipelineRegistryFactory.Create().Resolve(selected).RequestedPipelineIds.Should().Equal(PipelineIds.ItemOptionData);
        ApplicationConfiguration.CreateExportOptions().OutputPaths[PipelineIds.ItemOptionData]
            .Should().Be("item-option.json");
    }
}
