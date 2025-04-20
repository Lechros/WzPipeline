using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Domain;

namespace WzJson.Tests;

public class WzComparerR2Potential
{
    public WzComparerR2Potential()
    {
        props = new Dictionary<GearPropType, int>();
    }
    public int code;
    public int optionType;
    public int reqLevel;
    public Dictionary<GearPropType, int> props;
    public int weight;
    public string stringSummary;

    /// <summary>
    /// 指示潜能是否是附加潜能。
    /// </summary>
    public bool IsPotentialEx
    {
        get { return this.code / 1000 % 10 == 2; }
    }

    public override string ToString()
    {
        return this.code.ToString("d6") + " " + ConvertSummary()
               + (weight > 0 ? (" - " + weight) : null);
    }

    public string ConvertSummary()
    {
        if (string.IsNullOrEmpty(this.stringSummary))
            return null;
        List<string> types = new List<string>(this.props.Keys.Count);
        foreach (GearPropType k in this.props.Keys)
            types.Add(k.ToString());
        types.Sort((a, b) => b.Length.CompareTo(a.Length));
        string str = this.stringSummary;
        foreach (string s in types)
        {
            GearPropType t = (GearPropType)Enum.Parse(typeof(GearPropType), s);
            str = str.Replace("#" + s, this.props[t].ToString());
        }
        return str;
    }

    public static int GetPotentialLevel(int gearReqLevel)
    {
        if (gearReqLevel <= 0) return 1;
        else if (gearReqLevel >= 200) return 20;
        else return (gearReqLevel + 9) / 10;
    }

    public static bool CheckOptionType(int optionType, GearType gearType)
    {
        switch (optionType)
        {
            case 0: return true;
            case 10: return WzComparerR2Gear.IsWeapon(gearType) 
                            || WzComparerR2Gear.IsSubWeapon(gearType) // IsSubWeapon should return `true` for GearType.katara
                            || gearType == GearType.shield;
            case 11:
                return !CheckOptionType(10, gearType);
            case 20: return WzComparerR2Gear.IsSubWeapon(gearType)
                            || gearType == GearType.pants
                            || gearType == GearType.shoes
                            || gearType == GearType.cap
                            || gearType == GearType.coat
                            || gearType == GearType.longcoat
                            || gearType == GearType.glove
                            || gearType == GearType.cape
                            || gearType == GearType.belt
                            || gearType == GearType.shoulderPad;
            case 40: return gearType == GearType.faceAccessory
                            || gearType == GearType.eyeAccessory
                            || gearType == GearType.ring
                            || gearType == GearType.earrings
                            || gearType == GearType.pendant;
            case 51: return gearType == GearType.cap;
            case 52: return gearType == GearType.coat || gearType == GearType.longcoat;
            case 53: return gearType == GearType.pants;
            case 54: return gearType == GearType.glove;
            case 55: return gearType == GearType.shoes;
            default: return false;
        }
    }

    public static WzComparerR2Potential CreateFromNode(Wz_Node potentialNode, int pLevel)
    {
        WzComparerR2Potential wzComparerR2Potential = new WzComparerR2Potential();
        if (potentialNode == null || !Int32.TryParse(potentialNode.Text, out wzComparerR2Potential.code))
            return null;
        foreach (Wz_Node subNode in potentialNode.Nodes)
        {
            if (subNode.Text == "info")
            {
                foreach (Wz_Node infoNode in subNode.Nodes)
                {
                    switch (infoNode.Text)
                    {
                        case "optionType":
                            wzComparerR2Potential.optionType = Convert.ToInt32(infoNode.Value);
                            break;
                        case "reqLevel":
                            wzComparerR2Potential.reqLevel = Convert.ToInt32(infoNode.Value);
                            break;
                        case "weight":
                            wzComparerR2Potential.weight = Convert.ToInt32(infoNode.Value);
                            break;
                        case "string":
                            wzComparerR2Potential.stringSummary = Convert.ToString(infoNode.Value);
                            break;
                    }
                }
            }
            else if (subNode.Text == "level")
            {
                Wz_Node levelNode = subNode.FindNodeByPath(pLevel.ToString());
                if (levelNode != null)
                {
                    foreach (Wz_Node propNode in levelNode.Nodes)
                    {
                        try
                        {
                            GearPropType propType = (GearPropType)Enum.Parse(typeof(GearPropType), propNode.Text);
                            int value = (propType == GearPropType.face ? 0 : Convert.ToInt32(propNode.Value));
                            wzComparerR2Potential.props.Add(propType, value);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        return wzComparerR2Potential;
    }

    public static WzComparerR2Potential LoadFromWz(int optID, int optLevel, GlobalFindNodeFunction findNode)
    {
        Wz_Node itemWz = findNode("Item\\ItemOption.img");
        if (itemWz == null)
            return null;

        WzComparerR2Potential opt = CreateFromNode(itemWz.FindNodeByPath(optID.ToString("d6")), optLevel);
        return opt;
    }
}