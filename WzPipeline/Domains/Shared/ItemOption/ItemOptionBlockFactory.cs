using System.Threading.Tasks.Dataflow;
using WzPipeline.Shared;
using WzPipeline.Wz;

namespace WzPipeline.Domains.Shared.ItemOption;

public class ItemOptionBlockFactory(WzTree tree)
{
    public const string Pattern = "Item/ItemOption.img/*";

    public ISourceBlock<ItemOptionNode> CreateSource()
    {
        return tree.MatchNodes(Pattern).ToSourceBlock().Map(node => new ItemOptionNode(node));
    }

    public TransformBlock<ItemOptionNode, ItemOptionEntry> CreateParser()
    {
        return new TransformBlock<ItemOptionNode, ItemOptionEntry>(node =>
        {
            var itemOption = new ItemOptionEntry
            {
                Code = int.Parse(node.Id),
                OptionType = node.OptionType,
                ReqLevel = node.ReqLevel
            };
            var template = node.String;
            foreach (var (level, optionsDict) in node.LevelOptions)
            {
                optionsDict.TryGetValue("fixedGrade", out var fixedGrade);
                var option = new LevelOption
                {
                    String = InterpolateString(template, optionsDict),
                    Option = ConvertGearOption(optionsDict),
                    FixedGrade = fixedGrade != null ? int.Parse(fixedGrade) : null
                };
                itemOption.Level.Add(level, option);
            }

            return itemOption;
        });
    }

    public ITargetBlock<ItemOptionEntry> CreateDictionaryCollector(IDictionary<int, ItemOptionEntry> dictionary)
    {
        return new ActionBlock<ItemOptionEntry>(entry => { dictionary.Add(entry.Code, entry); });
    }

    private static string InterpolateString(string template, IDictionary<string, string> dict)
    {
        var names = dict.Keys.ToList();
        names.Sort((a, b) => b.Length.CompareTo(a.Length)); // Sort by longest first

        var str = template;
        foreach (var name in names) str = str.Replace($"#{name}", dict[name]);

        return str;
    }

    private static GearOption ConvertGearOption(IDictionary<string, string> dict)
    {
        if (dict.ContainsKey("boss"))
        {
            var value = int.Parse(dict["incDAMr"]);
            return new GearOption { BossDamage = value };
        }

        var gearOption = new GearOption();
        foreach (var (type, valueStr) in dict)
            if (int.TryParse(valueStr, out var value))
            {
                var prop = Enum.Parse<GearPropType>(type);
                gearOption.Add(prop, value);
            }

        return gearOption;
    }
}