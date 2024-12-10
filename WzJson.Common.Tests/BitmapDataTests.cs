using System.Drawing;
using FluentAssertions;
using WzJson.Common.Data;

namespace WzJson.Common.Tests;

[TestFixture]
public class BitmapDataTests
{
    [Test]
    public void Ctor_LabelAndPath_PropertiesReturn()
    {
        const string label = "label";
        const string path = "test-data.json";

        var data = new BitmapData(label, path);

        data.Label.Should().Be(label);
        data.Path.Should().Be(path);
    }

    [Test]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        using var value = new Bitmap(1, 1);
        var data = new BitmapData(path, path);

        data.Add(key, value);

        data[key].Should().Be(value);
    }
}