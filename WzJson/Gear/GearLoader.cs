using WzComparerR2.WzLib;
using WzComparerR2.Common;
using WzJson.Wz;
using Newtonsoft.Json;
using System.Diagnostics;

namespace WzJson.Gear
{
    public class GearLoader
    {
        static readonly string Character = @"Character";
        static readonly string StringEqp = @"String\Eqp.img";
        static readonly string[] parts = new string[]
        {
            "Accessory", "Android", "Cap", "Cape", "Coat", "Dragon", "Glove",
            "Longcoat", "Mechanic", "Pants", "Ring", "Shield", "Shoes", "Weapon",
        };

        public GearLoader(WzLoader wz)
        {
            this.wz = wz;
        }

        WzLoader wz;

        Dictionary<int, (string, string?)> nameDesc = new();
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
            Wz_Node stringWz = wz.openedWz!.WzNode.FindNodeByPath(StringEqp, true);
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
                    Wz_Node descNode = gearNode.FindNodeByPath("desc");
                    int id = int.Parse(gearNode.Text);
                    string name = (string)nameNode.Value;
                    string? desc = descNode?.Value as string;
                    nameDesc.Add(id, (name, desc));
                }
            }

            stringWz.GetValue<Wz_Image>().Unextract();
        }

        void LoadData()
        {
            Wz_Node chWz = wz.openedWz!.WzNode.FindNodeByPath(Character);

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
                if(!nameDesc.ContainsKey(id))
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
                gear.name = nameDesc[id].Item1;
                gear.desc = nameDesc[id].Item2;
                var iconOrigin = ResolveIcon(infoNode.FindNodeByPath("iconRaw"), infoNode.FindNodeByPath("icon"));
                gear.icon = iconOrigin.Item1;
                gear.origin = iconOrigin.Item2;
                gear.req = new();
                foreach(Wz_Node subNode in infoNode.Nodes)
                {
                    switch(subNode.Text)
                    {
                        case "cash": break;
                        case "icon": break;
                        case "iconRaw": break;
                        case "addition": break; // ignored for now
                        case "option":
                            gear.pots = new SpecialOption[subNode.Nodes.Count];
                            for(int i = 0; i < subNode.Nodes.Count; i++)
                            {
                                Wz_Node optNode = subNode.Nodes[i];
                                int option = Convert.ToInt32(optNode.FindNodeByPath("option").Value);
                                int level = Convert.ToInt32(optNode.FindNodeByPath("level").Value);
                                gear.pots[i] = new SpecialOption(option, level);
                            }
                            break;
                        default:
                            {
                                GearPropType type;
                                if(!int.TryParse(subNode.Text, out _) && Enum.TryParse(subNode.Text, out type))
                                {
                                    int value = Convert.ToInt32(subNode.Value);
                                    if((int)type < 100)
                                    {
                                        gear.options.Add(subNode.Text, value);
                                    }
                                    else if(1000 <= (int)type && (int)type < 1100)
                                    {
                                        switch(type)
                                        {
                                            case GearPropType.reqJob: gear.req.job = value; break;
                                            case GearPropType.reqSTR: gear.req.STR = value; break;
                                            case GearPropType.reqLUK: gear.req.LUK = value; break;
                                            case GearPropType.reqDEX: gear.req.DEX = value; break;
                                            case GearPropType.reqINT: gear.req.INT = value; break;
                                            case GearPropType.reqSpecJob: gear.req.specJob = value; break;
                                        }
                                    }
                                    else if(type == GearPropType.tuc)
                                    {
                                        gear.tuc = value;
                                    }
                                    else
                                    {
                                        gear.props.Add(subNode.Text, value);
                                    }
                                }
                            }
                            break;
                    }
                }
                return (id, gear);
            }
            finally
            {
                gearImg.Unextract();
            }
        }

        (int, int[]) ResolveIcon(Wz_Node iconRawNode, Wz_Node iconNode)
        {
            Wz_Node node = iconRawNode ?? iconNode;
            if(node == null)
            {
                return (0, new int[] { 0, 0 });
            }
            Wz_Uol uol;
            while((uol = node.GetValue<Wz_Uol>(null)) != null)
            {
                node = uol.HandleUol(node);
            }

            var linkNode = node.GetLinkedSourceNode(wz.Find);

            int id;
            int[] origin;
            if(linkNode != null)
            {
                id = Convert.ToInt32(linkNode.ParentNode.ParentNode.Text.Split('.')[0]);
            }
            else
            {
                id = Convert.ToInt32(node.ParentNode.ParentNode.Text.Split('.')[0]);
            }

            Wz_Node? originNode = node.FindNodeByPath("origin");
            Wz_Vector? vec = originNode?.GetValue<Wz_Vector>();
            if(vec != null)
            {
                origin = new int[] { vec.X, vec.Y };
            }
            else
            {
                origin = new int[] { 0, 0 };
            }

            return (id, origin);
        }
    }
}
