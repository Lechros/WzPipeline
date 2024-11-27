using System.Drawing;

namespace WzJson;

public class BitmapData : IData<Bitmap>
{
    public BitmapData(string name, IDictionary<string, Bitmap>? items = null)
    {
        Name = name;
        Items = items ?? new Dictionary<string, Bitmap>();
    }

    public string Name { get; }
    public IDictionary<string, Bitmap> Items { get; }
}