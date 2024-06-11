using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixes;

/// <summary>
/// Changes access modifier of 'set' accessor to private.
/// </summary>
internal class ToPrivate : ChangeSetAccessorStrategy
{
    /// <inheritdoc />
    public override string GetTitle(string format) => string.Format(format, "private set");

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } =
        syntax => syntax.WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));
}