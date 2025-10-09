using System.Text;
using System.Text.RegularExpressions;
using Sprache;
using WzComparerR2.WzLib;

namespace WzPipeline.Shared.Traverser.Internal;

internal class GlobNodeWalker(IWzProvider wzProvider, string pattern)
{
    private readonly List<IWzNodeSearcher> _searchers = GetSearchers(pattern);

    public IEnumerable<Wz_Node> GetNodes()
    {
        using var holder = new WzImageHolder();
        return Search(wzProvider.BaseNode, 0, holder);
    }

    private IEnumerable<Wz_Node> Search(Wz_Node node, int index, WzImageHolder holder)
    {
        if (!holder.EnsureOpened(ref node))
        {
            throw new ApplicationException("Image not opened: " + node.FullPath);
        }

        if (index == _searchers.Count)
        {
            yield return node;
            yield break;
        }

        foreach (var child in _searchers[index].Search(node))
        {
            foreach (var descendant in Search(child, index + 1, holder))
            {
                yield return descendant;
            }
        }
    }

    public int GetNodeCount()
    {
        using var holder = new WzImageHolder();
        return Count(wzProvider.BaseNode, 0, holder);
    }

    private int Count(Wz_Node node, int index, WzImageHolder holder)
    {
        if (!holder.EnsureOpened(ref node))
        {
            throw new ApplicationException("Image not opened: " + node.FullPath);
        }

        if (index == _searchers.Count - 1)
        {
            return _searchers[index].Count(node);
        }

        return _searchers[index].Search(node).Sum(child => Count(child, index + 1, holder));
    }

    private static List<IWzNodeSearcher> GetSearchers(string pattern)
    {
        var tokens = Parsers.Tokens.Parse(pattern).ToList();
        if (tokens.Count == 0)
            throw new ArgumentException("Invalid pattern: ", pattern);
        return tokens.Select<IPathToken, IWzNodeSearcher>(token =>
        {
            return token switch
            {
                LiteralToken literalToken => new LiteralSearcher(literalToken.Value),
                BraceToken braceToken => new SetSearcher(braceToken.Values),
                GlobToken globToken => new GlobSearcher(globToken.Pattern),
                _ => throw new NotImplementedException("Unknown token type: " + token.GetType())
            };
        }).ToList();
    }

    private class WzImageHolder : IDisposable
    {
        private Wz_Image? Image { get; set; }

        public bool EnsureOpened(ref Wz_Node node)
        {
            var image = node.GetNodeWzImage();
            if (image == null) return true;
            if (image.Name == Image?.Name) return true;
            if (!image.TryExtract()) return false;
            UnExtract();
            Image = image;
            node = image.Node;
            return true;
        }

        public void UnExtract()
        {
            Image?.Unextract();
            Image = null;
        }

        public void Dispose()
        {
            UnExtract();
        }
    }

    private interface IWzNodeSearcher
    {
        public IEnumerable<Wz_Node> Search(Wz_Node node);

        public int Count(Wz_Node node);
    }

    private class LiteralSearcher(string value) : IWzNodeSearcher
    {
        public IEnumerable<Wz_Node> Search(Wz_Node node)
        {
            var child = node.Nodes[value];
            if (child != null)
            {
                yield return child;
            }
        }

        public int Count(Wz_Node node)
        {
            var child = node.Nodes[value];
            return child != null ? 1 : 0;
        }
    }

    private class SetSearcher(IReadOnlyCollection<string> values) : IWzNodeSearcher
    {
        private readonly HashSet<string> set = [..values];

        public IEnumerable<Wz_Node> Search(Wz_Node node)
        {
            return node.Nodes.Where(child => set.Contains(child.Text));
        }

        public int Count(Wz_Node node)
        {
            return node.Nodes.Count(child => set.Contains(child.Text));
        }
    }

    private class GlobSearcher(string pattern) : IWzNodeSearcher
    {
        private readonly Regex? regex = CreateRegex(pattern);

        public IEnumerable<Wz_Node> Search(Wz_Node node)
        {
            if (regex == null)
            {
                return node.Nodes;
            }

            return node.Nodes.Where(child => regex.IsMatch(child.Text));
        }

        public int Count(Wz_Node node)
        {
            if (regex == null)
            {
                return node.Nodes.Count;
            }

            return node.Nodes.Count(child => regex.IsMatch(child.Text));
        }

        private static Regex? CreateRegex(string globPattern)
        {
            if (globPattern == "*")
            {
                return null;
            }

            var sb = new StringBuilder("^");
            foreach (var c in globPattern)
            {
                switch (c)
                {
                    case '*': sb.Append(".*"); break;
                    case '?': sb.Append("."); break;
                    default:
                        if (".\\+^$[]{}()|".Contains(c))
                            sb.Append('\\');
                        sb.Append(c);
                        break;
                }
            }

            sb.Append("$");
            return new Regex(sb.ToString(), RegexOptions.Compiled);
        }
    }
}