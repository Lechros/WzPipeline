using WzJson.Core.Pipeline.Abstractions;

namespace WzJson.Core.Pipeline.Graph;

public interface IRepositoryNode : IGraphNode
{
    public IRepository Repository { get; }

    public void AddChild(IConverterNode node);
}