using System.Diagnostics.CodeAnalysis;
using System.Text;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace WzJson.Common;

public class WzProvider : IWzProvider
{
    static readonly string RelativeBaseWzPath = Path.Join("Data", "Base", "Base.wz");

    private readonly string baseWzPath;
    private Wz_Structure? openedWz;

    static WzProvider()
    {
        Wz_Structure.DefaultEncoding = Encoding.Default;
        Wz_Structure.DefaultAutoDetectExtFiles = true;
        Wz_Structure.DefaultImgCheckDisabled = true;
        Wz_Structure.DefaultWzVersionVerifyMode = WzVersionVerifyMode.Fast;
    }

    public WzProvider(string maplePath)
    {
        baseWzPath = Path.Join(maplePath, RelativeBaseWzPath);
        if (!File.Exists(baseWzPath))
        {
            throw new FileNotFoundException($"Base.wz not found in {RelativeBaseWzPath}");
        }
    }

    public Wz_Node BaseNode
    {
        get
        {
            EnsureLoaded();
            return openedWz.WzNode;
        }
    }

    public GlobalFindNodeFunction FindNodeFunction => FindNode;

    [MemberNotNull(nameof(openedWz))]
    private void EnsureLoaded()
    {
        if (openedWz == null)
        {
            OpenWz(baseWzPath);
        }
    }

    [MemberNotNull(nameof(openedWz))]
    private void OpenWz(string wzFilePath)
    {
        if (openedWz != null)
        {
            foreach (Wz_File wzf in openedWz.wz_files)
            {
                if (String.Compare(wzf.Header.FileName, wzFilePath, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return;
                }
            }
        }

        Wz_Structure wz = new Wz_Structure();
        if (wz.IsKMST1125WzFormat(wzFilePath))
        {
            wz.LoadKMST1125DataWz(wzFilePath);
        }
        else
        {
            wz.Load(wzFilePath, true);
        }

        openedWz = wz;
    }

    private Wz_Node? FindNode(string fullPath)
    {
        EnsureLoaded();

        Wz_Type wzType = Wz_Type.Unknown;
        // Wz_File? wzFile = null;
        Wz_Node? wzNode = null;

        string[]? fullPathList = null;
        if (!string.IsNullOrEmpty(fullPath))
        {
            fullPathList = fullPath.Split('/', '\\');
            wzType = Enum.TryParse(fullPathList[0], true, out wzType) ? wzType : Wz_Type.Unknown;
        }

        List<Wz_Node> preSearch = new List<Wz_Node>();
        if (wzType != Wz_Type.Unknown)
        {
            Wz_File? baseWz = null;
            bool find = false;
            foreach (Wz_File wzf in openedWz.wz_files)
            {
                if (wzf.Type == wzType)
                {
                    preSearch.Add(wzf.Node);
                    find = true;
                }

                if (wzf.Type == Wz_Type.Base)
                {
                    baseWz = wzf;
                }
            }

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

        if (fullPathList == null || fullPathList.Length <= 1)
        {
            if (wzType != Wz_Type.Unknown && preSearch.Count > 0)
            {
                wzNode = preSearch[0];
                // wzFile = preSearch[0].Value as Wz_File;
            }

            return wzNode;
        }

        if (preSearch.Count <= 0)
        {
            return wzNode;
        }

        foreach (var wzFileNode in preSearch)
        {
            var searchNode = wzFileNode;
            for (int i = 1; i < fullPathList.Length && searchNode != null; i++)
            {
                searchNode = searchNode.Nodes[fullPathList[i]];
                Wz_Image? img = searchNode.GetValueEx<Wz_Image>(null);
                if (img != null)
                {
                    searchNode = img.TryExtract() ? img.Node : null;
                }
            }

            if (searchNode != null)
            {
                wzNode = searchNode;
                // wzFile = wzFileNode.Value as Wz_File;
                return wzNode;
            }
        }

        return null;
    }
}