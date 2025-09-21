using WzJson.V2.Core.Stereotype;
using WzJson.V2.Domains.Gear;
using WzJson.V2.Domains.Gear.Models;

namespace WzJson.V2.Domains.ItemOption;

public class ItemOptionConverter : AbstractConverter<IItemOptionNode, ItemOptionEntry>
{
    public override ItemOptionEntry? Convert(IItemOptionNode node)
    {
        var itemOption = new ItemOptionEntry
        {
            Code = int.Parse(node.Id),
            OptionType = node.OptionType,
            ReqLevel = node.ReqLevel,
        };
        var template = node.String;
        foreach (var (level, optionsDict) in node.LevelOptions)
        {
            var option = new LevelOption
            {
                String = InterpolateString(template, optionsDict),
                Option = ConvertGearOption(optionsDict)
            };
            itemOption.Level.Add(level, option);
        }

        return itemOption;
    }

    private string InterpolateString(string template, IDictionary<string, string> dict)
    {
        var names = dict.Keys.ToList();
        names.Sort((a, b) => b.Length.CompareTo(a.Length)); // Sort by longest first

        var str = template;
        foreach (var name in names)
        {
            str = str.Replace($"#{name}", dict[name]);
        }

        return str;
    }

    private GearOption ConvertGearOption(IDictionary<string, string> dict)
    {
        if (dict.ContainsKey("boss"))
        {
            var value = int.Parse(dict["incDAMr"]);
            return new GearOption { BossDamage = value };
        }

        var gearOption = new GearOption();
        foreach (var (type, valueStr) in dict)
        {
            if (int.TryParse(valueStr, out var value))
            {
                var prop = Enum.Parse<GearPropType>(type);
                gearOption.Add(prop, value);
            }
        }

        return gearOption;
    }
}