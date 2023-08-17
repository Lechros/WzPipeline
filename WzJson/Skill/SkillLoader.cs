using System.Diagnostics;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using WzJson.Wz;

namespace WzJson.Skill
{
    internal class SkillLoader
    {
        static readonly string Skill = @"Skill";
        static readonly string SkillCanvas = @"Skill\_Canvas";

        public SkillLoader(WzLoader wz)
        {
            this.wz = wz;
        }

        WzLoader wz;

        public bool Load()
        {
            return true;
        }

        public bool SaveIcons(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            foreach(Wz_Node itemNode in GetSkillCanvasNodes())
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
                    throw new Exception(node.Text + " is not valid id");
                }
                Wz_Node iconNode = node.FindNodeByPath("icon");
                if(iconNode == null)
                {
                    throw new Exception("icon node doesn't exist.");
                }

                Wz_Png png = (Wz_Png)iconNode.Value;
                png.ExtractPng().Save(Path.Join(parentPath, $"{id}.png"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to save icon on " + node.Text);
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<Wz_Node> GetSkillCanvasNodes()
        {
            Wz_Node canvasWz = wz.openedWz!.WzNode.FindNodeByPath(SkillCanvas);

            foreach(Wz_Node imgNode in canvasWz.Nodes)
            {
                if(!char.IsDigit(imgNode.Text[0])) continue;

                Wz_Image img = (Wz_Image)imgNode.Value;
                if(!img.TryExtract()) continue;

                foreach(Wz_Node skillNode in img.Node.FindNodeByPath("skill").Nodes)
                {
                    yield return skillNode;
                }
            }
        }

        private bool IsMaybeSkillNode(Wz_Node node)
        {
            string s = node.Text;
            for(int i = 0; i < s.Length; i++)
            {
                if(s[i] > '9' || s[i] < '0')
                    return false;
            }
            return true;
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
