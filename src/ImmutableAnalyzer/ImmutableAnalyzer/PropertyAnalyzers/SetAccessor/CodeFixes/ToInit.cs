using Microsoft.CodeAnalysis.CSharp;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixes;

/// <summary>
/// Changes 'set' accessor to 'init'.
/// </summary>
internal class ToInit : ChangeSetAccessorStrategy
{
    /// <inheritdoc />
    public override string GetTitle(string format) => string.Format(format, "init");

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } =
        syntax => syntax.WithKeyword(SyntaxFactory.Token(SyntaxKind.InitKeyword));
}