using WzJson.Core.Stereotype;

namespace WzJson.Domains.String;

public class NameDescConverter : AbstractConverter<INameDescNode, NameDesc>
{
    public override NameDesc? Convert(INameDescNode node)
    {
        return new NameDesc
        {
            Id = node.Id,
            Name = node.Name,
            Desc = node.Desc
        };
    }
}