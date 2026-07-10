using System.Text.RegularExpressions;
using Sprache;
using WzComparerR2.WzLib;

namespace WzPipeline.Wz;

public class WzMatcher(string pattern)
{
    private readonly List<IPathSegmentSelector> selectors = CreateSelectors(pattern);

    public static IEnumerable<Wz_Node> Match(Wz_Node node, string pattern)
    {
        return new WzMatcher(pattern).Match(node);
    }

    public IEnumerable<Wz_Node> Match(Wz_Node node)
    {
        return Match(node, 0);
    }

    private IEnumerable<Wz_Node> Match(Wz_Node node, int depth)
    {
        if (depth == selectors.Count)
        {
            yield return node;
            yield break;
        }

        var wzImg = EnsureExtracted(node);
        if (wzImg != null)
        {
            node = wzImg.Node;
        }

        foreach (var child in selectors[depth].Select(node))
        {
            foreach (var result in Match(child, depth + 1))
            {
                yield return result;
            }
        }
    }

    private Wz_Image? EnsureExtracted(Wz_Node node)
    {
        var wzImg = node.GetNodeWzImage();
        if (wzImg != null)
        {
            if (!wzImg.TryExtract(out var ex))
            {
                throw ex;
            }
        }

        return wzImg;
    }

    private static List<IPathSegmentSelector> CreateSelectors(string pattern)
    {
        var tokens = Parsers.Tokens.Parse(pattern).ToList();

        if (tokens.Count == 0)
        {
            throw new ArgumentException($@"Invalid pattern: {pattern}", nameof(pattern));
        }

        var requiredLiteral = true;
        var selectors = new List<IPathSegmentSelector>(tokens.Count);

        foreach (var token in tokens)
        {
            var selector = token switch
            {
                LiteralToken literal => new LiteralSegmentSelector(literal.Value, requiredLiteral),
                BraceToken brace => new SetSegmentSelector(brace.Values),
                GlobToken glob => GlobSegmentSelector.Create(glob.Pattern),
                _ => throw new NotSupportedException($"Unknown token type: {token.GetType()}")
            };

            selectors.Add(selector);

            if (token is not LiteralToken)
            {
                requiredLiteral = false;
            }
        }

        return selectors;
    }

    private interface IPathSegmentSelector
    {
        IEnumerable<Wz_Node> Select(Wz_Node node);
    }

    private sealed class LiteralSegmentSelector(string value, bool required) : IPathSegmentSelector
    {
        public IEnumerable<Wz_Node> Select(Wz_Node node)
        {
            var child = node.Nodes[value];

            if (child != null)
            {
                yield return child;
                yield break;
            }

            if (required)
            {
                throw new InvalidOperationException(
                    $"Required path segment '{value}' was not found under '{node.FullPath}'.");
            }
        }
    }

    private sealed class SetSegmentSelector(IEnumerable<string> values) : IPathSegmentSelector
    {
        private readonly HashSet<string> values = [..values];

        public IEnumerable<Wz_Node> Select(Wz_Node node)
        {
            return node.Nodes.Where(child => values.Contains(child.Text));
        }
    }

    private sealed class AllChildrenSegmentSelector : IPathSegmentSelector
    {
        public IEnumerable<Wz_Node> Select(Wz_Node node)
        {
            return node.Nodes;
        }
    }

    private sealed class GlobSegmentSelector : IPathSegmentSelector
    {
        private readonly Regex regex;

        private GlobSegmentSelector(string pattern, bool compiled)
        {
            regex = new Regex(
                "^" + Regex.Escape(pattern)
                    .Replace(@"\*", ".*")
                    .Replace(@"\?", ".") + "$",
                compiled ? RegexOptions.Compiled : RegexOptions.None);
        }

        public static IPathSegmentSelector Create(string pattern, bool compiled = false)
        {
            return pattern == "*"
                ? new AllChildrenSegmentSelector()
                : new GlobSegmentSelector(pattern, compiled);
        }

        public IEnumerable<Wz_Node> Select(Wz_Node node)
        {
            return node.Nodes.Where(child => regex.IsMatch(child.Text));
        }
    }
}