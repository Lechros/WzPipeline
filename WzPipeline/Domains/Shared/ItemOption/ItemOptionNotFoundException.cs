namespace WzPipeline.Domains.Shared.ItemOption;

public class ItemOptionNotFoundException(int optionCode, int level)
    : Exception($"Item Option was not found. OptionCode={optionCode}, Level={level}.");