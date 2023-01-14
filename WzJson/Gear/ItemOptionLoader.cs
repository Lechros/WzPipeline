using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WzComparerR2.WzLib;
using WzJson.Wz;

namespace WzJson.Gear
{
    public class ItemOptionLoader
    {
        static readonly string ItemOption = @"Item\ItemOption.img";

        public ItemOptionLoader(WzLoader wz)
        {
            this.wz = wz;
        }

        WzLoader wz;

        SortedDictionary<int, ItemOption> options = new();

        public bool Load()
        {
            Wz_Node ioWz = wz.openedWz!.WzNode.FindNodeByPath(ItemOption, true);

            foreach(Wz_Node itemOptionNode in ioWz.Nodes)
            {
                var (code, opt) = Parse(itemOptionNode) ?? (0, null);
                if(opt != null)
                {
                    options.Add(code, opt);
                }
            }
            ioWz.GetValue<Wz_Image>().Unextract();
            return true;
        }

        public bool Save(string path)
        {
            if(options.Count <= 0)
            {
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            JsonSerializer serializer = new();
            using(StreamWriter sw = new(path))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, options);
            }
            return true;
        }

        (int, ItemOption)? Parse(Wz_Node node)
        {
            if(!int.TryParse(node.Text.Split('.')[0], out int code))
            {
                return null;
            }

            Wz_Node infoNode = node.FindNodeByPath("info");
            if(infoNode == null)
            {
                return null;
            }
            Wz_Node levelNode = node.FindNodeByPath("level");
            if(levelNode == null)
            {
                return null;
            }

            ItemOption option = new();

            foreach(Wz_Node subNode in infoNode.Nodes)
            {
                switch(subNode.Text)
                {
                    case "optionType":
                        option.optionType = Convert.ToInt32(subNode.Value);
                        break;
                    case "reqLevel":
                        option.reqLevel = Convert.ToInt32(subNode.Value);
                        break;
                    case "string":
                        option.@string = Convert.ToString(subNode.Value);
                        break;
                }
            }
            foreach(Wz_Node optionNode in levelNode.Nodes)
            {
                foreach(Wz_Node subNode in optionNode.Nodes)
                {
                    option.addOption(optionNode.Text, subNode.Text, new JValue(subNode.Value));
                }
            }
            return (code, option);
        }
    }
}
