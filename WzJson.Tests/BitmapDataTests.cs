using System.Drawing;

namespace WzJson.Tests;

[TestClass]
public class BitmapDataTests
{
    [TestMethod]
    public void Ctor_Path_PropertiesReturn()
    {
        const string path = "test-data.json";

        var data = new BitmapData(path);

        Assert.AreEqual(path, data.Path);
    }

    [TestMethod]
    public void Ctor_PathAndDictionary_PropertiesReturn()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        using var value = new Bitmap(1, 1);

        var data = new BitmapData(path, new Dictionary<string, Bitmap> { [key] = value });

        Assert.AreEqual(path, data.Path);
        Assert.AreEqual(1, data.Items.Count);
        Assert.AreEqual(value, data.Items[key]);
    }

    [TestMethod]
    public void Add_NewKeyValuePair_ItemsContainsPair()
    {
        const string path = "test-data.json";
        const string key = "1234567";
        using var value = new Bitmap(1, 1);
        var data = new BitmapData(path);

        data.Add(key, value);

        Assert.AreEqual(value, data.Items[key]);
    }
}