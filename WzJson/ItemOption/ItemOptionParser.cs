namespace WzJson.ItemOption;

public class ItemOptionParser : IWzParser
{
    public const string ItemOptionJsonName = "item-option.json";

    private readonly ItemOptionNodeRepository itemOptionNodeRepository;


    public ItemOptionParser(ItemOptionNodeRepository itemOptionNodeRepository)
    {
        this.itemOptionNodeRepository = itemOptionNodeRepository;
    }

    public IList<IData> Parse()
    {
        var converter = new ItemOptionConverter(ItemOptionJsonName);
        var data = converter.NewData();

        foreach (var node in itemOptionNodeRepository.GetNodes())
        {
            var name = converter.GetNodeName(node);
            var item = converter.ConvertNode(node, name);
            if (item != null)
                data.Add(name, item);
        }

        return new List<IData> { data };
    }
}