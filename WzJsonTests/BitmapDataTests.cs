using System.Drawing;
using WzJson;

namespace WzJsonTests;

[TestClass]
public class BitmapDataTests
{
    [TestMethod]
    public void Ctor_Path_PropertiesReturn()
    {
        const string Path = "test-data.json";

        var data = new BitmapData(Path);

        Assert.AreEqual(Path, data.Path);
    }

    [TestMethod]
    public void Ctor_PathAndDictionary_PropertiesReturn()
    {
        const string Path = "test-data.json";
        const string Key = "1234567";
        Bitmap Value = new(1, 1);

        var data = new BitmapData(Path, new Dictionary<string, Bitmap> { [Key] = Value });

        Assert.AreEqual(Path, data.Path);
        Assert.AreEqual(1, data.Items.Count);
        Assert.AreEqual(Value, data.Items[Key]);
    }
}