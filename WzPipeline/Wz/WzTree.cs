using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using WzComparerR2.WzLib;

namespace WzPipeline.Wz;

public class WzTree
{
    private readonly Wz_Structure wzs;

    private WzTree(Wz_Structure wzStructure)
    {
        wzs = wzStructure;
    }

    public Wz_Node BaseNode => wzs.WzNode;

    public static WzTree Load(string baseWzPath)
    {
        if (!File.Exists(baseWzPath))
        {
            throw new FileNotFoundException($"Base.wz not found at: {baseWzPath}");
        }

        var wzs = new Wz_Structure()
        {
            TextEncoding = Encoding.Default,
            AutoDetectExtFiles = true,
            ImgCheckDisabled = true,
        };
        if (wzs.IsKMST1125WzFormat(baseWzPath))
        {
            wzs.LoadKMST1125DataWz(baseWzPath);
        }
        else
        {
            wzs.Load(baseWzPath, true);
        }

        return new WzTree(wzs);
    }

    public Wz_Node? FindNode(string path, Wz_File? wzFile = null)
    {
        string[]? fullPath = null;
        Wz_Type wzType = Wz_Type.Unknown;
        if (!string.IsNullOrEmpty(path)) //用path作为输入参数
        {
            fullPath = path.Split('/', '\\');
            wzType = Enum.TryParse(fullPath[0], true, out wzType) ? wzType : Wz_Type.Unknown;
        }

        var wzNode = FindNodeInternal(fullPath, wzType, ref wzFile);
        return wzNode ?? wzFile?.Node;
    }

    public IEnumerable<Wz_Node> MatchNodes(string pattern)
    {
        return WzMatcher.Match(BaseNode, pattern);
    }

    private Wz_Node? FindNodeInternal(string[]? fullPath, Wz_Type wzType, ref Wz_File? wzFile)
    {
        List<Wz_Node> preSearch = new List<Wz_Node>();
        if (wzType != Wz_Type.Unknown) //用wztype作为输入参数
        {
            IEnumerable<Wz_Structure> preSearchWz = wzFile?.WzStructure != null
                ? Enumerable.Repeat(wzFile.WzStructure, 1)
                : Enumerable.Repeat(wzs, 1);
            foreach (var wzs in preSearchWz)
            {
                Wz_File? baseWz = null;
                bool find = false;
                foreach (Wz_File wzf in wzs.wz_files)
                {
                    if (wzf.Type == wzType)
                    {
                        if (wzf.Node.Nodes.Count <= 0)
                        {
                            continue;
                        }

                        preSearch.Add(wzf.Node);
                        find = true;
                        //e.WzFile = wz_f;
                    }

                    if (wzf.Type == Wz_Type.Base)
                    {
                        baseWz = wzf;
                    }
                }

                // detect data.wz
                if (baseWz != null && !find)
                {
                    string key = wzType.ToString();
                    foreach (Wz_Node node in baseWz.Node.Nodes)
                    {
                        if (node.Text == key && node.Nodes.Count > 0)
                        {
                            preSearch.Add(node);
                        }
                    }
                }
            }
        }

        if (fullPath == null || fullPath.Length <= 1)
        {
            if (wzType != Wz_Type.Unknown && preSearch.Count > 0) //返回wzFile
            {
                wzFile = preSearch[0].Value as Wz_File;
                return preSearch[0];
            }
        }

        if (preSearch.Count <= 0)
        {
            return null;
        }

        if (fullPath == null)
        {
            throw new ApplicationException("Unreachable state or a bug in WzComparerR2");
        }

        foreach (var wzFileNode in preSearch)
        {
            var searchNode = wzFileNode;
            for (int i = 1; i < fullPath.Length && searchNode != null; i++)
            {
                searchNode = searchNode.Nodes[fullPath[i]];
                var img = searchNode.GetValueEx<Wz_Image?>(null);
                if (img != null)
                {
                    searchNode = img.TryExtractThreadSafe() ? img.Node : null;
                }
            }

            if (searchNode != null)
            {
                wzFile = wzFileNode.Value as Wz_File;
                return searchNode;
            }
        }

        //寻找失败
        wzFile = null;
        return null;
    }
}