using System.Diagnostics;
using System.Text;
using WzComparerR2.WzLib;

namespace WzJson.Wz
{
    public class WzLoader
    {
        static readonly string BaseWzName = Path.Join("Data", "Base", "Base.wz");
        static readonly string MapleExeName = "MapleStory.exe";

        static WzLoader()
        {
            Wz_Structure.DefaultEncoding = Encoding.Default;
            Wz_Structure.DefaultAutoDetectExtFiles = true;
            Wz_Structure.DefaultImgCheckDisabled = true;
            Wz_Structure.DefaultWzVersionVerifyMode = WzVersionVerifyMode.Fast;
        }

        public WzLoader()
        {
            VersionInfo = "1.2.000";
        }

        public Wz_Structure? openedWz;
        public string VersionInfo;

        public bool Loaded { get => openedWz != null; }

        public bool Load(string maplePath)
        {
            string basePath = Path.Join(maplePath, BaseWzName);
            string exePath = Path.Join(maplePath, MapleExeName);
            if (!File.Exists(basePath) || !File.Exists(exePath))
            {
                return false;
            }

            OpenWz(basePath);
            var info = FileVersionInfo.GetVersionInfo(exePath);
            VersionInfo = info.FileMajorPart + ".2." + info.FileMinorPart;
            return true;
        }

        bool OpenWz(string wzFilePath)
        {
            if (openedWz != null)
            {
                foreach (Wz_File wzf in openedWz.wz_files)
                {
                    if (string.Compare(wzf.Header.FileName, wzFilePath, true) == 0)
                    {
                        return false;
                    }
                }
            }

            Wz_Structure wz = new();
            if (wz.IsKMST1125WzFormat(wzFilePath))
            {
                wz.LoadKMST1125DataWz(wzFilePath);
            }
            else
            {
                wz.Load(wzFilePath, true);
            }

            openedWz = wz;
            return true;
        }

        public Wz_Node? Find(string fullPath)
        {
            Wz_Type wzType = Wz_Type.Unknown;
            Wz_File? wzFile = null;
            Wz_Node? wzNode = null;

            string[] path = null;
            if(!string.IsNullOrEmpty(fullPath))
            {
                path = fullPath.Split('/', '\\');
                if(!Enum.TryParse<Wz_Type>(path[0], true, out wzType))
                {
                    wzType = Wz_Type.Unknown;
                }
            }

            List<Wz_Node> preSearch = new List<Wz_Node>();
            if(wzType != Wz_Type.Unknown)
            {
                Wz_File baseWz = null;
                bool find = false;
                foreach(Wz_File wzf in openedWz.wz_files)
                {
                    if(wzf.Type == wzType)
                    {
                        preSearch.Add(wzf.Node);
                        find = true;
                    }
                    if(wzf.Type == Wz_Type.Base)
                    {
                        baseWz = wzf;
                    }
                }

                if(baseWz != null && !find)
                {
                    string key = wzType.ToString();
                    foreach(Wz_Node node in baseWz.Node.Nodes)
                    {
                        if(node.Text == key && node.Nodes.Count > 0)
                        {
                            preSearch.Add(node);
                        }
                    }
                }
            }

            if(path == null || path.Length <= 1)
            {
                if(wzType != Wz_Type.Unknown && preSearch.Count > 0)
                {
                    wzNode = preSearch[0];
                    // wzFile = preSearch[0].Value as Wz_File;
                }
                return wzNode;
            }

            if(preSearch.Count <= 0)
            {
                return wzNode;
            }

            foreach(var wzFileNode in preSearch)
            {
                var searchNode = wzFileNode;
                for(int i = 1; i < path.Length && searchNode != null; i++)
                {
                    searchNode = searchNode.Nodes[path[i]];
                    var img = searchNode.GetValueEx<Wz_Image>(null);
                    if(img != null)
                    {
                        searchNode = img.TryExtract() ? img.Node : null;
                    }
                }

                if(searchNode != null)
                {
                    wzNode = searchNode;
                    wzFile = wzFileNode.Value as Wz_File;
                    return wzNode ?? wzFile?.Node;
                }
            }

            return null;
        }
    }
}
