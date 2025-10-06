using Sprache;

namespace WzJson.V2.Shared.Traverser.Internal;

internal static class Parsers
{
    private static readonly Parser<IPathToken> Brace =
        from lbrace in Parse.Char('{')
        from options in Parse.CharExcept('}').AtLeastOnce().Text()
        from rbrace in Parse.Char('}')
        select (IPathToken)new BraceToken(options.Split(',').Select(s => s.Trim()).ToList());

    private static readonly Parser<IPathToken> Glob =
        Parse.CharExcept(new[] { '/', '{', '}' })
            .AtLeastOnce()
            .Text()
            .Where(text => text.Contains('*') || text.Contains('?'))
            .Select(text => (IPathToken)new GlobToken(text));

    private static readonly Parser<IPathToken> Literal =
        Parse.CharExcept(new[] { '/', '{', '}', '*', '?' })
            .AtLeastOnce()
            .Text()
            .Select(text => (IPathToken)new LiteralToken(text));

    public static readonly Parser<IEnumerable<IPathToken>> Tokens =
        Brace.Or(Glob).Or(Literal).DelimitedBy(Parse.Char('/'));
}