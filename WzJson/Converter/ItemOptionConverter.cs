using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Domain;
using WzJson.Model;

namespace WzJson.Converter;

public class ItemOptionConverter : AbstractNodeConverter<ItemOption>
{
    public override string GetNodeKey(Wz_Node node) => WzUtility.GetNodeCode(node);

    public override ItemOption? Convert(Wz_Node node, string _)
    {
        var infoNode = node.Nodes["info"] ?? throw new InvalidDataException("info node not found");;
        var levelListNode = node.Nodes["level"] ?? throw new InvalidDataException("level node not found");

        var itemOption = new ItemOption();
        string? stringTemplate = null;
        foreach (var subNode in infoNode.Nodes)
        {
            switch (subNode.Text)
            {
                case "optionType":
                    itemOption.OptionType = subNode.GetValue<int>();
                    break;
                case "reqLevel":
                    itemOption.ReqLevel = subNode.GetValue<int>();
                    break;
                case "string":
                    stringTemplate = subNode.GetValue<string>();
                    break;
            }
        }

        if (stringTemplate == null) throw new InvalidDataException("string node not found");

        foreach (var levelNode in levelListNode.Nodes)
        {
            var level = int.Parse(levelNode.Text);
            var info = new ItemOption.LevelInfo
            {
                String = InterpolateString(stringTemplate, levelNode),
                Option = ConvertToGearOption(levelNode)
            };
            itemOption.Level.Add(level, info);
        }

        return itemOption;
    }

    private string InterpolateString(string stringTemplate, Wz_Node levelNode)
    {
        var args = levelNode.Nodes.ToDictionary(node => node.Text, node => node.GetValue<string>());
        var types = args.Keys.ToList();
        types.Sort((a, b) => b.Length.CompareTo(a.Length));
        var str = stringTemplate;
        foreach (var type in types)
            str = str.Replace($"#{type}", args[type]);
        return str;
    }

    private GearOption ConvertToGearOption(Wz_Node levelNode)
    {
        // bossDamage is stored in wz as { boss=1, incDAMr=(value) }
        if (levelNode.Nodes["boss"] != null)
        {
            var damNode = levelNode.Nodes["incDAMr"];
            if (damNode == null) throw new NotImplementedException();
            var value = damNode.GetValue<int>();
            return new GearOption { BossDamage = value };
        }

        var gearOption = new GearOption();
        foreach (var propNode in levelNode.Nodes)
        {
            var propType = Enum.Parse<GearPropType>(propNode.Text);
            var optionName = propType.GetGearOptionName();
            if (optionName != null)
            {
                var value = propNode.GetValue<int>();
                gearOption[optionName] = value;
            }
        }

        return gearOption;
    }
}