using System.Drawing;
using WzJson;

namespace WzJsonTests;

[TestClass]
public class BitmapDataTests
{
    [TestMethod]
    public void Ctor_NameAndDictionary_PropertiesReturn()
    {
        const string Name = "test-data.json";
        const string Key = "1234567";
        Bitmap Value = new(1, 1);

        var data = new BitmapData(Name, new Dictionary<string, Bitmap>
        {
            [Key] = Value
        });

        Assert.AreEqual(Name, data.Name);
        Assert.AreEqual(1, data.Items.Count);
        Assert.AreEqual(Value, data.Items[Key]);
    }
}