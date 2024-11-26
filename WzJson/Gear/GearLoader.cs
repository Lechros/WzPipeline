using WzComparerR2.WzLib;
using WzComparerR2.Common;
using WzJson.Wz;
using Newtonsoft.Json;
using System.Diagnostics;
using WzJson.SimapleGear;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

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
        SortedDictionary<int, int[]> origins = new();
        SortedDictionary<int, int[]> rawOrigins = new();

        public bool Load()
        {
            if(gears.Count == 0 || origins.Count == 0 || rawOrigins.Count == 0)
            {
                LoadString();
                LoadData();
            }
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

        public bool SaveOrigins(string path, bool raw = false)
        {
            if(origins.Count <= 0)
            {
                return false;
            }

            JsonSerializer serializer = new();
            using(StreamWriter sw = new(path))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, raw ? rawOrigins : origins);
            }

            return true;
        }

        public bool SaveIcons(string path, bool raw = false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            foreach(Wz_Node gearNode in GetGearNodes())
            {
                SaveIcon(gearNode, path, raw);
            }

            return true;
        }

        void SaveIcon(Wz_Node node, string parentPath, bool raw = false)
        {
            try
            {
                if(!int.TryParse(node.Text.Split('.')[0], out int id))
                {
                    return;
                }
                Wz_Node infoNode = node.FindNodeByPath("info");
                var iconNode = raw ? infoNode.FindNodeByPath("iconRaw") : infoNode.FindNodeByPath("icon");
                Wz_Png png = (Wz_Png)ResolveIconNode(iconNode).Value;
                png.ExtractPng().Save(Path.Join(parentPath, $"{id}.png"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to save icon on " + node.Text);
                Console.WriteLine(ex.Message);
            }
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
            foreach(Wz_Node gearNode in GetGearNodes())
            {
                var (id, gear) = Parse(gearNode) ?? (-1, null);
                if(id == -1) continue;
                gears.Add(id, gear);
                var origin = ParseOrigin(gearNode);
                if(origin != null)
                {
                    origins.Add(id, origin);
                }
                var rawOrigin = ParseOrigin(gearNode, true);
                if(rawOrigin != null)
                {
                    rawOrigins.Add(id, rawOrigin);
                }
            }
        }

        int[]? ParseOrigin(Wz_Node gearNode, bool raw = false)
        {
            Wz_Node infoNode = gearNode.FindNodeByPath("info");
            var iconNode = raw ? infoNode.FindNodeByPath("iconRaw") : infoNode.FindNodeByPath("icon");
            var originNode = iconNode.FindNodeByPath("origin");
            Wz_Vector? vec = originNode?.GetValue<Wz_Vector>();
            return vec == null ? null : new int[] { vec.X, vec.Y };
        }

        (int, Gear)? Parse(Wz_Node node)
        {
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
            gear.req = new();
            foreach(Wz_Node subNode in infoNode.Nodes)
            {
                switch(subNode.Text)
                {
                    case "cash": break;
                    case "icon":
                    case "iconRaw":
                        if(gear.icon == 0)
                        {
                            gear.icon = Convert.ToInt32(ResolveIconNode(subNode).ParentNode.ParentNode.Text.Split('.')[0]);
                        }
                        break;
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
                                        case GearPropType.reqLevel: gear.req.level = value; break;
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
                                else if(type == GearPropType.Etuc)
                                {
                                    gear.etuc = value;
                                }
                                else
                                {
                                    if(value == 0 && subNode.Nodes.Count > 0)
                                    {
                                        value = 1;
                                    }
                                    gear.props.Add(subNode.Text, value);
                                }
                            }
                        }
                        break;
                }
            }
            return (id, gear);
        }

        public IEnumerable<Wz_Node> GetGearNodes()
        {
            Wz_Node chWz = wz.openedWz!.WzNode.FindNodeByPath(Character);

            foreach(string part in parts)
            {
                Wz_Node partNode = chWz.FindNodeByPath(part);
                foreach(Wz_Node gearNode in partNode.Nodes)
                {
                    if(gearNode.Text == "_Canvas") continue;
                    Wz_Image gearImg = (Wz_Image)gearNode.Value;
                    try
                    {
                        if(gearImg.TryExtract())
                        {
                            yield return gearImg.Node;
                        }
                    }
                    finally
                    {
                        gearImg.Unextract();
                    }
                }
            }
        }

        Wz_Node? ResolveIconNode(Wz_Node? iconNode)
        {
            Wz_Node? node = iconNode;
            if(node == null)
            {
                return null;
            }
            Wz_Uol uol;
            while((uol = node.GetValue<Wz_Uol>(null)) != null)
            {
                node = uol.HandleUol(node);
            }

            return node.GetLinkedSourceNode(wz.Find);
        }
    }
}
