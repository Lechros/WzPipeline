using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WzComparerR2.WzLib;
using WzJson.Common;
using WzJson.Gear;

namespace WzJson.Soul
{
    internal class SoulLoader
    {
        static readonly string SoulPath = @"Item\Consume\0259.img";
        static readonly string StringConsume = @"String\Consume.img";

        public SoulLoader(WzProvider wz)
        {
            this.wz = wz;
        }

        WzProvider wz;

        Dictionary<int, (string, string?)> nameDesc = new();
        SortedDictionary<int, Soul> souls = new();

        public bool Load()
        {
            LoadString();
            LoadData();
            return true;
        }

        public bool Save(string path)
        {
            if(souls.Count <= 0)
            {
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            JsonSerializer serializer = new();
            using(StreamWriter sw = new(path))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, souls);
            }
            return true;
        }

        void LoadString()
        {
            Wz_Node stringWz = wz.BaseNode.FindNodeByPath(StringConsume, true);
            stringWz.GetValue<Wz_Image>().TryExtract();

            foreach(Wz_Node itemNode in stringWz.Nodes)
            {
                int id = int.Parse(itemNode.Text);
                if(id / 10000 != 259)
                {
                    continue;
                }

                Wz_Node nameNode = itemNode.FindNodeByPath("name");
                if(nameNode == null)
                {
                    continue;
                }
                Wz_Node descNode = itemNode.FindNodeByPath("desc");
                string name = (string)nameNode.Value;
                string? desc = descNode?.Value as string;
                nameDesc.Add(id, (name, desc));
            }

            stringWz.GetValue<Wz_Image>().Unextract();
        }

        void LoadData()
        {
            Wz_Node soulWz = wz.BaseNode.FindNodeByPath(SoulPath, true);
            foreach(Wz_Node node in soulWz.Nodes)
            {
                var (id, soul) = Parse(node) ?? (0, null);
                if(soul != null)
                {
                    souls.Add(id, soul);
                }
            }
        }

        (int, Soul)? Parse(Wz_Node node)
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

            var (name, desc) = nameDesc[id];
            if(!name.EndsWith("소울"))
            {
                return null;
            }

            Wz_Node tradeBlockNode = infoNode.FindNodeByPath("tradeBlock");
            bool isTradeBlock = tradeBlockNode != null && Convert.ToInt32(tradeBlockNode.Value) != 0;
            bool isMagnificent = name.StartsWith("위대한");
            if(isMagnificent && isTradeBlock || !isMagnificent && !isTradeBlock)
            {
                return null;
            }

            string mobName = TrimSoulName(name);

            Soul soul = new();
            soul.name = name;
            if(isMagnificent)
            {
                soul.skill = SoulData.skills[mobName].Item2 ?? throw new NotImplementedException();
            }
            else
            {
                soul.skill = SoulData.skills[mobName].Item1;
            }
            soul.multiplier = SoulData.multiplier[mobName];
            soul.magnificent = isMagnificent;
            if(isMagnificent)
            {
                soul.options = GetMagnificentOptions(mobName);
            }
            else if(desc != null)
            {
                var result = Regex.Match(desc, @"\\n#c추가 잠재능력 : ([^#]+?)#\\n");
                if(result.Success && result.Groups.Count > 0)
                {
                    soul.option = ParseSummary(result.Groups[1].Value);
                }
            }

            return (id, soul);
        }

        string TrimSoulName(string fullName)
        {
            var result = Regex.Match(fullName, @"[가-힣]+ ([\w 가-힣]+?)의? ?소울");
            return result.Groups[1].Value;
        }

        Dictionary<string, int> ParseSummary(string summary)
        {
            Dictionary<string, int> options = new();
            var result = Regex.Match(summary, @"([\w 가-힣]+?) \+(\d+)");
            if(result.Success)
            {
                string name = result.Groups[1].Value;
                int value = int.Parse(result.Groups[2].Value);
                switch(name)
                {
                    case "힘":
                        options.Add(GearPropType.incSTR.ToString(), value);
                        break;
                    case "민첩":
                        options.Add(GearPropType.incDEX.ToString(), value);
                        break;
                    case "지능":
                        options.Add(GearPropType.incINT.ToString(), value);
                        break;
                    case "행운":
                        options.Add(GearPropType.incLUK.ToString(), value);
                        break;
                    case "올스탯":
                        options.Add(GearPropType.incAllStat.ToString(), value);
                        break;
                    case "MHP":
                    case "MaxHP":
                        options.Add(GearPropType.incMHP.ToString(), value);
                        break;
                    case "MaxMP":
                        options.Add(GearPropType.incMMP.ToString(), value);
                        break;
                    case "공격력":
                        options.Add(GearPropType.incPAD.ToString(), value);
                        break;
                    case "마력":
                        options.Add(GearPropType.incMAD.ToString(), value);
                        break;
                    case "방어력 무시":
                        options.Add(GearPropType.imdR.ToString(), value);
                        break;
                    case "보스 공격력":
                        options.Add(GearPropType.bdR.ToString(), value);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return options;
        }

        Dictionary<string, Dictionary<string, int>> GetMagnificentOptions(string mobName)
        {
            Dictionary<string, Dictionary<string, int>> options;
            switch(SoulData.grade[mobName])
            {
                case 1:
                    options = new()
                    {
                        ["PAD"] = new() { [GearPropType.incPADr.ToString()] = 3 },
                        ["MAD"] = new() { [GearPropType.incMADr.ToString()] = 3 },
                        ["allStat"] = new() { [GearPropType.statR.ToString()] = 5 },
                        ["MHP"] = new() { [GearPropType.incMHP.ToString()] = 2000 },
                        ["cr"] = new() { [GearPropType.incCr.ToString()] = 12 },
                        ["bdR"] = new() { [GearPropType.bdR.ToString()] = 7 },
                        ["imdR"] = new() { [GearPropType.imdR.ToString()] = 7 },
                    };
                    break;
                case 2:
                    options = new()
                    {
                        ["PAD"] = new() { [GearPropType.incPAD.ToString()] = 10 },
                        ["MAD"] = new() { [GearPropType.incMAD.ToString()] = 10 },
                        ["allStat"] = new() { [GearPropType.incAllStat.ToString()] = 20 },
                        ["MHP"] = new() { [GearPropType.incMHP.ToString()] = 1500 },
                        ["cr"] = new() { [GearPropType.incCr.ToString()] = 10 },
                        ["bdR"] = new() { [GearPropType.bdR.ToString()] = 5 },
                        ["imdR"] = new() { [GearPropType.imdR.ToString()] = 5 },
                    };
                    break;
                case 3:
                case 4:
                    if(mobName == "반 레온")
                    {
                        options = new()
                        {
                            ["PAD"] = new() { [GearPropType.incPAD.ToString()] = 7 },
                            ["MAD"] = new() { [GearPropType.incMAD.ToString()] = 7 },
                            ["allStat"] = new() { [GearPropType.incAllStat.ToString()] = 15 },
                            ["MHP"] = new() { [GearPropType.incMHP.ToString()] = 1200 },
                            ["cr"] = new() { [GearPropType.incCr.ToString()] = 7 },
                            ["bdR"] = new() { [GearPropType.bdR.ToString()] = 4 },
                            ["imdR"] = new() { [GearPropType.imdR.ToString()] = 4 },
                        };
                    }
                    else
                    {
                        options = new()
                        {
                            ["PAD"] = new() { [GearPropType.incPAD.ToString()] = 8 },
                            ["MAD"] = new() { [GearPropType.incMAD.ToString()] = 8 },
                            ["allStat"] = new() { [GearPropType.incAllStat.ToString()] = 17 },
                            ["MHP"] = new() { [GearPropType.incMHP.ToString()] = 1300 },
                            ["cr"] = new() { [GearPropType.incCr.ToString()] = 8 },
                            ["bdR"] = new() { [GearPropType.bdR.ToString()] = 4 },
                            ["imdR"] = new() { [GearPropType.imdR.ToString()] = 4 },
                        };
                        switch(mobName)
                        {
                            case "모카딘":
                                options["PAD"][GearPropType.incPAD.ToString()] = 10;
                                break;
                            case "카리아인":
                                options["MAD"][GearPropType.incMAD.ToString()] = 10;
                                break;
                            case "CQ57":
                                options["cr"][GearPropType.incCr.ToString()] = 10;
                                break;
                            case "줄라이":
                                options["MHP"][GearPropType.incMHP.ToString()] = 1500;
                                break;
                            case "플레드":
                                options["allStat"][GearPropType.incAllStat.ToString()] = 20;
                                break;
                        }
                    }
                    break;
                case 5:
                    options = new()
                    {
                        ["PAD"] = new() { [GearPropType.incPAD.ToString()] = 6 },
                        ["MAD"] = new() { [GearPropType.incMAD.ToString()] = 6 },
                        ["allStat"] = new() { [GearPropType.incAllStat.ToString()] = 12 },
                        ["MHP"] = new() { [GearPropType.incMHP.ToString()] = 1100 },
                        ["cr"] = new() { [GearPropType.incCr.ToString()] = 6 },
                        ["bdR"] = new() { [GearPropType.bdR.ToString()] = 3 },
                        ["imdR"] = new() { [GearPropType.imdR.ToString()] = 3 },
                    };
                    break;
                case 6:
                    options = new()
                    {
                        ["PAD"] = new() { [GearPropType.incPAD.ToString()] = 5 },
                        ["MAD"] = new() { [GearPropType.incMAD.ToString()] = 5 },
                        ["allStat"] = new() { [GearPropType.incAllStat.ToString()] = 10 },
                        ["MHP"] = new() { [GearPropType.incMHP.ToString()] = 1000 },
                        ["cr"] = new() { [GearPropType.incCr.ToString()] = 5 },
                        ["bdR"] = new() { [GearPropType.bdR.ToString()] = 3 },
                        ["imdR"] = new() { [GearPropType.imdR.ToString()] = 3 },
                    };
                    break;
                default:
                    throw new InvalidDataException();
            }
            return options;
        }
    }
}
