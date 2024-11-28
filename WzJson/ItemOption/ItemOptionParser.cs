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
        var data = converter.Convert(itemOptionNodeRepository.GetNodes());
        return new List<IData> { data };
    }
}