using WzJson.Core.Abstractions;

namespace WzJson.Core.Pipeline;

public interface IRepositoryNode : IGraphNode
{
    public IRepository Repository { get; }

    public void AddChild(IConverterNode node);
}