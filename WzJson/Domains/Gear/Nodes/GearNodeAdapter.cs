using System.Drawing;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Domains.Icon;

namespace WzJson.Domains.Gear.Nodes;

public class GearNodeAdapter(Wz_Node node, Wz_Node infoNode, GlobalFindNodeFunction findNode) : IGearNode
{
    public static GearNodeAdapter? Create(Wz_Node node, GlobalFindNodeFunction findNode)
    {
        var infoNode = node.Nodes["info"];
        return infoNode == null ? null : new GearNodeAdapter(node, infoNode, findNode);
    }

    public string Id => node.Text.Split('.')[0].TrimStart('0');

    public string IconId => Id;

    public Bitmap? Icon => IconUtility.GetIconImage(infoNode, "icon", findNode);

    public Point? IconOrigin => IconUtility.GetIconOrigin(infoNode, "icon", findNode);

    public string RawIconId => Id;

    public Bitmap? RawIcon => IconUtility.GetIconImage(infoNode, "iconRaw", findNode);

    public Point? RawIconOrigin => IconUtility.GetIconOrigin(infoNode, "iconRaw", findNode);

    public bool IsCash
    {
        get
        {
            var cashNode = infoNode.Nodes["cash"];
            return cashNode != null && cashNode.GetValue<int>() != 0;
        }
    }

    public (int OptionCode, int Level)[]? Options
    {
        get
        {
            var optionNode = infoNode.Nodes["option"];
            return optionNode?.Nodes
                .Select(n => (
                    n.Nodes["option"].GetValue<int>(),
                    n.Nodes["level"].GetValue<int>()))
                .ToArray();
        }
    }

    public IEnumerable<(string Type, int Value)> Properties
    {
        get
        {
            foreach (var propNode in infoNode.Nodes)
            {
                switch (propNode.Text)
                {
                    case "icon":
                    case "iconRaw":
                    case "addition":
                    case "option":
                        break;
                    case "onlyUpgrade":
                        yield return (propNode.Text, 1);
                        break;
                    default:
                        yield return (propNode.Text, propNode.GetValue<int>());
                        break;
                }
            }
        }
    }
}