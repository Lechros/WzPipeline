using Newtonsoft.Json;
using System.IO;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Wz;

namespace WzJson.Item
{
    public class ItemLoader
    {
        static readonly string Item = @"Item";
        static readonly string[] parts = new string[]
        {
            "Cash", "Consume", "Etc"
        };

        public ItemLoader(WzLoader wz)
        {
            this.wz = wz;
        }

        WzLoader wz;

        SortedDictionary<int, int[]> origins = new();

        public bool Load()
        {
            foreach(Wz_Node itemNode in GetItemNodes())
            {
                var result = Parse(itemNode);
                if(result != null)
                {
                    origins.Add(int.Parse(itemNode.Text), result);
                }
            }
            return true;
        }

        int[]? Parse(Wz_Node node, bool raw = false)
        {
            if(!int.TryParse(node.Text, out int id))
            {
                return null;
            }
            Wz_Node? infoNode = node.FindNodeByPath("info");
            var iconNode = ResolveIconNode(raw ? infoNode.FindNodeByPath("iconRaw") : infoNode.FindNodeByPath("icon"));

            Wz_Node? originNode = iconNode.FindNodeByPath("origin");
            Wz_Vector? vec = originNode?.GetValue<Wz_Vector>();
            if(vec != null)
            {
                return new int[] { vec.X, vec.Y };
            }
            else
            {
                return new int[] { 0, 0 };
            }
        }

        public bool Save(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            JsonSerializer serializer = new();
            using(StreamWriter sw = new(path))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, origins);
            }
            return true;
        }

        public bool SaveIcons(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            foreach(Wz_Node itemNode in GetItemNodes())
            {
                SaveIcon(itemNode, path);
            }
            return true;
        }

        void SaveIcon(Wz_Node node, string parentPath, bool raw = false)
        {
            try
            {
                if(!int.TryParse(node.Text, out int id))
                {
                    Console.WriteLine("Skipping: " + node.Text);
                }
                Wz_Node infoNode = node.FindNodeByPath("info");
                var iconNode = ResolveIconNode(raw ? infoNode.FindNodeByPath("iconRaw") : infoNode.FindNodeByPath("icon"));

                Wz_Png png = (Wz_Png)iconNode.Value;
                png.ExtractPng().Save(Path.Join(parentPath, $"{id}.png"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to save icon on " + node.Text);
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<Wz_Node> GetItemNodes()
        {
            Wz_Node itemWz = wz.openedWz!.WzNode.FindNodeByPath(Item);

            foreach(string part in parts)
            {
                Wz_Node partNode = itemWz.FindNodeByPath(part);
                foreach(Wz_Node itemListNode in partNode.Nodes)
                {
                    Wz_Image itemListImg = (Wz_Image)itemListNode.Value;
                    if(!itemListImg.TryExtract())
                    {
                        continue;
                    }

                    foreach(Wz_Node itemNode in itemListImg.Node.Nodes)
                    {
                        yield return itemNode;
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
