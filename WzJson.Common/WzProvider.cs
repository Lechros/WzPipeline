using System.Diagnostics.CodeAnalysis;
using System.Text;
using WzComparerR2.WzLib;

namespace WzJson.Common;

public class WzProvider : IWzProvider
{
    private Wz_Structure openedWz;

    static WzProvider()
    {
        Wz_Structure.DefaultEncoding = Encoding.Default;
        Wz_Structure.DefaultAutoDetectExtFiles = true;
        Wz_Structure.DefaultImgCheckDisabled = true;
        Wz_Structure.DefaultWzVersionVerifyMode = WzVersionVerifyMode.Fast;
    }

    public WzProvider(string baseWzPath)
    {
        if (!File.Exists(baseWzPath))
            throw new FileNotFoundException($"Wz path {baseWzPath} does not exist.");
        OpenWz(baseWzPath);
    }

    public Wz_Node BaseNode => openedWz.WzNode;

    [MemberNotNull(nameof(openedWz))]
    private void OpenWz(string wzFilePath)
    {
        var wz = new Wz_Structure();
        if (wz.IsKMST1125WzFormat(wzFilePath))
            wz.LoadKMST1125DataWz(wzFilePath);
        else
            wz.Load(wzFilePath, true);

        openedWz = wz;
    }

    public Wz_Node? FindNode(string fullPath)
    {
        var wzType = Wz_Type.Unknown;
        Wz_Node? wzNode = null;

        string[]? fullPathList = null;
        if (!string.IsNullOrEmpty(fullPath))
        {
            fullPathList = fullPath.Split('/', '\\');
            wzType = Enum.TryParse(fullPathList[0], true, out wzType) ? wzType : Wz_Type.Unknown;
        }

        List<Wz_Node> preSearch = new();
        if (wzType != Wz_Type.Unknown)
        {
            Wz_File? baseWz = null;
            var find = false;
            foreach (var wzf in openedWz.wz_files)
            {
                if (wzf.Type == wzType)
                {
                    preSearch.Add(wzf.Node);
                    find = true;
                }

                if (wzf.Type == Wz_Type.Base) baseWz = wzf;
            }

            if (baseWz != null && !find)
            {
                var key = wzType.ToString();
                foreach (var node in baseWz.Node.Nodes)
                {
                    if (node.Text == key && node.Nodes.Count > 0)
                        preSearch.Add(node);
                }
            }
        }

        if (fullPathList == null || fullPathList.Length <= 1)
        {
            if (wzType != Wz_Type.Unknown && preSearch.Count > 0)
                wzNode = preSearch[0];
            return wzNode;
        }

        if (preSearch.Count <= 0) return wzNode;

        foreach (var wzFileNode in preSearch)
        {
            var searchNode = wzFileNode;
            for (var i = 1; i < fullPathList.Length && searchNode != null; i++)
            {
                searchNode = searchNode.Nodes[fullPathList[i]];
                var img = searchNode.GetValueEx<Wz_Image?>(null);
                if (img != null) searchNode = img.TryExtract() ? img.Node : null;
            }

            if (searchNode != null)
            {
                wzNode = searchNode;
                return wzNode;
            }
        }

        return null;
    }
}