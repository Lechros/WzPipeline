using WzComparerR2.WzLib;
using WzComparerR2.Common;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WzJson.Soul;
using WzJson.Gear;

namespace WzJson.SetItem
{
    public class SetItemLoader
    {
        static readonly string SetItemPath = @"Etc\SetItemInfo.img";
        static readonly string ItemOptionPath = @"Item\ItemOption.img";

        public SetItemLoader(WzProvider wz)
        {
            this.wz = wz;
            itemOptionNode = wz.BaseNode.FindNodeByPath(ItemOptionPath, true);
        }

        WzProvider wz;

        Wz_Node itemOptionNode;

        SortedDictionary<int, SetItem> setItems = new();

        public bool Load()
        {
            Wz_Node setItemWz = wz.BaseNode.FindNodeByPath(SetItemPath, true);
            foreach(Wz_Node node in setItemWz.Nodes)
            {
                var (id, soul) = Parse(node) ?? (0, null);
                if(soul != null)
                {
                    setItems.Add(id, soul);
                }
            }

            return true;
        }

        public bool Save(string path)
        {
            if(setItems.Count <= 0)
            {
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            JsonSerializer serializer = new();
            using(StreamWriter sw = new(path))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, setItems);
            }
            return true;
        }

        (int, SetItem)? Parse(Wz_Node node)
        {
            if(!int.TryParse(node.Text, out int id))
            {
                return null;
            }

            SetItem set = new();
            foreach(Wz_Node subNode in node.Nodes)
            {
                switch(subNode.Text)
                {
                    case "setItemName": set.name = Convert.ToString(subNode.Value);
                        break;
                    case "ItemID":
                        foreach(Wz_Node item in subNode.Nodes)
                        {
                            if(item.Nodes.Count == 0) // ItemID: [id, id, ...]
                            {
                                set.itemIDs.Add(Convert.ToInt32(item.Value));
                            }
                            else // ItemID: {1: {id, id}, 2: {id, id}, ...}
                            {
                                foreach(Wz_Node itemDirNode in item.Nodes)
                                {
                                    switch(itemDirNode.Text)
                                    {
                                        case "representName": break;
                                        case "typeName": break;
                                        case "byGender": break;
                                        default:
                                            if(int.TryParse(itemDirNode.Text, out int temp) && temp > 0)
                                                set.itemIDs.Add(Convert.ToInt32(itemDirNode.Value));
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case "Effect":
                        foreach(Wz_Node effect in subNode.Nodes)
                        {
                            string partCount = Convert.ToString(effect.Text);
                            Dictionary<string, int> effectDict = new();
                            set.effects[partCount] = effectDict;
                            foreach(Wz_Node prop in effect.Nodes)
                            {
                                switch(prop.Text)
                                {
                                    case "Option":
                                        foreach(Wz_Node optionNode in prop.Nodes)
                                        {
                                            int option = Convert.ToInt32(optionNode.FindNodeByPath("option").Value);
                                            int level = Convert.ToInt32(optionNode.FindNodeByPath("level").Value);
                                            string key; int value;
                                            (key, value) = ItemOptionToProp(option, level);
                                            if(key != null)
                                                effectDict.Add(key, value);
                                        }
                                        break;
                                    default:
                                        if(Enum.TryParse(prop.Text, out GearPropType type))
                                        {
                                            effectDict.Add(prop.Text, Convert.ToInt32(prop.Value));
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case "jokerPossible":
                        if(Convert.ToInt32(subNode.Value) != 0)
                            set.jokerPossible = true;
                        break;
                    case "zeroWeaponJokerPossible":
                        if(Convert.ToInt32(subNode.Value) != 0)
                            set.zeroWeaponJokerPossible = true;
                        break;
                    default:
                        // throw new NotImplementedException($"{subNode.Text}: 지원하지 않는 세트 아이템 옵션입니다.");
                        break;
                }
            }

            return (id, set);
        }

        (string, int) ItemOptionToProp(int option, int level)
        {
            // referenced itemOptions: 603, 10241, 20396, 30107, 30291, 30601, 30602, 32070, 40070, 40116, 60020, 60023, 60024
            Wz_Node optionNode = itemOptionNode.FindNodeByPath(@$"{option:D6}\level\{level}");
            if(optionNode.Nodes.Count == 1)
            {
                Wz_Node node = optionNode.Nodes[0];
                return (node.Text, Convert.ToInt32(node.Value));
            }
            else if(optionNode.Nodes.Count == 2)
            {
                Wz_Node bossNode = optionNode.FindNodeByPath("boss");
                if(bossNode != null)
                {
                    Wz_Node damNode = optionNode.FindNodeByPath("incDAMr");
                    return ("incBDR", Convert.ToInt32(damNode.Value));
                }
            }
            return (null, 0);
        }
    }
}
