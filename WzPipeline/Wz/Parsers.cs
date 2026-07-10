using Sprache;

namespace WzPipeline.Wz;

internal static class Parsers
{
    private static readonly Parser<IPathToken> Brace =
        from lbrace in Parse.Char('{')
        from options in Parse.CharExcept('}').AtLeastOnce().Text()
        from rbrace in Parse.Char('}')
        select new BraceToken(options.Split(',').Select(s => s.Trim()).ToList());

    private static readonly Parser<IPathToken> Glob =
        Parse.CharExcept(['/', '{', '}'])
            .AtLeastOnce()
            .Text()
            .Where(text => text.Contains('*') || text.Contains('?'))
            .Select(text => new GlobToken(text));

    private static readonly Parser<IPathToken> Literal =
        Parse.CharExcept(['/', '{', '}', '*', '?'])
            .AtLeastOnce()
            .Text()
            .Select(text => new LiteralToken(text));

    public static readonly Parser<IEnumerable<IPathToken>> Tokens =
        Brace.Or(Glob).Or(Literal).DelimitedBy(Parse.Char('/'));
}