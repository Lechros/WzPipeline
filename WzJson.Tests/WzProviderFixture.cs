using WzJson.Common;

namespace WzJson.Tests;

public class WzProviderFixture : IDisposable
{
    private const string MaplePath = @"C:\Nexon\Maple";

    public WzProviderFixture()
    {
        WzProvider = new WzProvider(MaplePath);
    }

    public IWzProvider WzProvider { get; private set; }

    public void Dispose()
    {
    }
}