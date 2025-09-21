namespace WzJson.V2.Shared.Traverser.Internal;

internal interface IPathToken;

internal record LiteralToken(string Value) : IPathToken;

internal record BraceToken(IReadOnlyList<string> Values) : IPathToken;

internal record GlobToken(string Pattern) : IPathToken;
