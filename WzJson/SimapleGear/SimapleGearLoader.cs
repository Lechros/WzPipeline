using WzComparerR2.WzLib;
using WzComparerR2.Common;
using Newtonsoft.Json;
using System.Diagnostics;

namespace WzJson.SimapleGear
{
    public class SimapleGearLoader
    {
        static readonly string Character = @"Character";
        static readonly string StringEqp = @"String\Eqp.img";
        static readonly string[] parts = new string[]
        {
            "Accessory", "Android", "Cap", "Cape", "Coat", "Dragon", "Glove",
            "Longcoat", "Mechanic", "Pants", "Ring", "Shield", "Shoes", "Weapon",
        };

        public SimapleGearLoader(WzProvider wz)
        {
            this.wz = wz;
        }

        WzProvider wz;

        Dictionary<int, string> nameDict = new();
        SortedDictionary<int, Gear> gears = new();

        public bool Load()
        {
            LoadString();
            LoadData();
            return true;
        }

        public bool Save(string path)
        {
            if(gears.Count <= 0)
            {
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            JsonSerializer serializer = new();
            using(StreamWriter sw = new(path))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, gears);
            }
            return true;
        }

        void LoadString()
        {
            Wz_Node stringWz = wz.BaseNode.FindNodeByPath(StringEqp, true);
            stringWz.GetValue<Wz_Image>().TryExtract();
            Wz_Node eqpNode = stringWz.Nodes[0];

            foreach(Wz_Node partNode in eqpNode.Nodes)
            {
                foreach(Wz_Node gearNode in partNode.Nodes)
                {
                    Wz_Node nameNode = gearNode.FindNodeByPath("name");
                    if(nameNode == null)
                    {
                        continue;
                    }
                    int id = int.Parse(gearNode.Text);
                    string name = (string)nameNode.Value;
                    nameDict.Add(id, name);
                }
            }

            stringWz.GetValue<Wz_Image>().Unextract();
        }

        void LoadData()
        {
            Wz_Node chWz = wz.BaseNode.FindNodeByPath(Character);

            foreach(string part in parts)
            {
                Wz_Node partNode = chWz.FindNodeByPath(part);
                foreach(Wz_Node gearNode in partNode.Nodes)
                {
                    var (id, gear) = Parse(gearNode) ?? (0, null);
                    if(gear != null)
                    {
                        gears.Add(id, gear);
                    }
                }
            }
        }

        (int, Gear)? Parse(Wz_Node gearNode)
        {
            Wz_Image gearImg = (Wz_Image)gearNode.Value;
            try
            {
                if(!gearImg.TryExtract())
                {
                    return null;
                }

                Wz_Node node = gearImg.Node;
                if(!int.TryParse(node.Text.Split('.')[0], out int id))
                {
                    return null;
                }
                if(!nameDict.ContainsKey(id))
                {
                    return null;
                }
                Wz_Node infoNode = node.FindNodeByPath("info");
                if(infoNode == null)
                {
                    return null;
                }
                Wz_Node cashNode = infoNode.FindNodeByPath("cash");
                if(cashNode != null && Convert.ToInt32(cashNode.Value) != 0)
                {
                    return null;
                }

                Gear gear = new();
                gear.name = nameDict[id];
                foreach(Wz_Node subNode in infoNode.Nodes)
                {
                    switch(subNode.Text)
                    {
                        case "cash": break;
                        case "icon": break;
                        case "iconRaw": break;
                        case "addition":
                            foreach(Wz_Node addiNode in subNode.Nodes)
                            {
                                if(addiNode.Text == "critical")
                                {
                                    foreach(Wz_Node addiSubNode in addiNode.Nodes)
                                    {
                                        switch(addiSubNode.Text)
                                        {
                                            case "prob":
                                                gear.critical_rate += Convert.ToInt32(addiSubNode.Value);
                                                break;
                                            case "damage":
                                                gear.critical_damage += Convert.ToInt32(addiSubNode.Value);
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        case "option": break; // ignore default potentials
                        default:
                            {
                                GearPropType type;
                                if(!int.TryParse(subNode.Text, out _) && Enum.TryParse(subNode.Text, out type))
                                {
                                    int value = Convert.ToInt32(subNode.Value);
                                    switch(type)
                                    {
                                        case GearPropType.reqLevel: gear.req_level += value; break;
                                        case GearPropType.reqJob: gear.req_job += value; break;
                                        case GearPropType.tuc: gear.tuc += value; break;
                                        case GearPropType.Etuc: gear.etuc += value; break;

                                        case GearPropType.incSTR: gear.STR += value; break;
                                        case GearPropType.incDEX: gear.DEX += value; break;
                                        case GearPropType.incINT: gear.INT += value; break;
                                        case GearPropType.incLUK: gear.LUK += value; break;

                                        case GearPropType.incSTRr: gear.STR_multiplier += value; break;
                                        case GearPropType.incDEXr: gear.DEX_multiplier += value; break;
                                        case GearPropType.incINTr: gear.INT_multiplier += value; break;
                                        case GearPropType.incLUKr: gear.LUK_multiplier += value; break;

                                        case GearPropType.incPAD: gear.attack_power += value; break;
                                        case GearPropType.incMAD: gear.magic_attack += value; break;

                                        case GearPropType.damR: gear.damage_multiplier += value; break;
                                        case GearPropType.bdR: gear.boss_damage_multiplier += value; break;
                                        case GearPropType.imdR: gear.ignored_defence += value; break;

                                        case GearPropType.incMHP: gear.MHP += value; break;
                                        case GearPropType.incMMP: gear.MMP += value; break;

                                        case GearPropType.setItemID: gear.set_item_id = value; break;
                                        case GearPropType.bossReward: gear.boss_reward = value; break;
                                        case GearPropType.superiorEqp: gear.superior_eqp = value; break;
                                        case GearPropType.jokerToSetItem: gear.joker_to_set_item = value; break;
                                        case GearPropType.blockGoldHammer: gear.block_hammer = value; break;

                                        case GearPropType.onlyUpgrade:
                                            gear.block_star = 1;
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
                if(id == 1099015) // 루인 포스실드 최종뎀
                {
                    gear.final_damage_multiplier += 10;
                }
                if(gear.tuc <= 0)
                {
                    gear.block_star = 1;
                }
                return (id, gear);
            }
            finally
            {
                gearImg.Unextract();
            }
        }
    }
}
