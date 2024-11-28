using System.Drawing;

namespace WzJson;

public class BitmapData : IData
{
    public BitmapData(string path, IDictionary<string, Bitmap>? items = null)
    {
        Path = path;
        Items = items ?? new Dictionary<string, Bitmap>();
    }

    public string Path { get; }
    public IDictionary<string, Bitmap> Items { get; }
}