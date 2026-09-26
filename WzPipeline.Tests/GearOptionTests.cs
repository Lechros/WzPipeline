using WzPipeline.Domains.Shared;

namespace WzPipeline.Tests;

public class GearOptionTests
{
    [Test]
    public void ToString_FormatsStatsWithoutReadingSetterOnlyProperties()
    {
        var option = new GearOption { AllStatsSetter = 10, AllStatRatesSetter = 5 };

        var result = option.ToString();

        result.Should().Contain(
            "Str=10, Dex=10, Int=10, Luk=10, StrRate=5, DexRate=5, IntRate=5, LukRate=5");
    }
}
