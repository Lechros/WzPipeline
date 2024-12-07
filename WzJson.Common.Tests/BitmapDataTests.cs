using System.Drawing;
using WzJson.Common.Data;

namespace WzJson.Common.Tests;

public class BitmapDataTests
{
    [Fact]
    public void Ctor_Path_PropertiesReturn()
    {
        const string path = "test-data.json";

        var data = new BitmapData(path, path);

        Assert.Equal(path, data.Path);
    }

    [Fact]
    public void Ctor_PathAndDictionary_PropertiesReturn()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        using var value = new Bitmap(1, 1);

        var data = new BitmapData(path, path, new Dictionary<string, Bitmap> { [key] = value });

        Assert.Equal(path, data.Path);
        Assert.Equal(1, data.Items.Count);
        Assert.Equal(value, data.Items[key]);
    }

    [Fact]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        using var value = new Bitmap(1, 1);
        var data = new BitmapData(path, path);

        data.Add(key, value);

        Assert.Equal(value, data.Items[key]);
    }
}