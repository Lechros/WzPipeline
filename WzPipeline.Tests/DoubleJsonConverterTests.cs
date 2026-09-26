using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WzPipeline.Application.Shared.Json;

namespace WzPipeline.Tests;

public class DoubleJsonConverterTests
{
    [TestCase(1.0, "1")]
    [TestCase(-1.0, "-1")]
    [TestCase(0.0, "0")]
    [TestCase(0.5, "0.5")]
    [TestCase(7.5, "7.5")]
    [TestCase(2147483648.0, "2147483648")]
    [TestCase(9223372036854775808.0, "9223372036854775808")]
    [SetCulture("fr-FR")]
    public void Serialize_WritesWholeNumbersWithoutDecimalSuffix(double value, string expected)
    {
        using var services = new ServiceCollection().AddApplicationJson().BuildServiceProvider();
        using var writer = new StringWriter(CultureInfo.InvariantCulture);

        services.GetRequiredService<JsonSerializer>().Serialize(writer, value);

        writer.ToString().Should().Be(expected);
    }

    [TestCase("1", 1.0)]
    [TestCase("7.5", 7.5)]
    public void Deserialize_ReadsNumbersAsDouble(string json, double expected)
    {
        using var services = new ServiceCollection().AddApplicationJson().BuildServiceProvider();
        using var reader = new JsonTextReader(new StringReader(json));

        services.GetRequiredService<JsonSerializer>().Deserialize<double>(reader).Should().Be(expected);
    }
}
