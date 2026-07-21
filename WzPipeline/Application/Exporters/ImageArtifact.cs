using System.Drawing;

namespace WzPipeline.Application.Exporters;

public sealed record ImageArtifact(string Id, Image Image) : IDisposable
{
    public void Dispose()
    {
        Image.Dispose();
    }
}