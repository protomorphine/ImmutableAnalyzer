using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixStrategies;

/// <summary>
/// Changes access modifier of 'set' accessor to private.
/// </summary>
internal class ToPrivateSetStrategy : ChangeSetAccessorStrategy
{
    /// <inheritdoc />
    public override string GetTitle(string format) => string.Format(format, "private set");

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } =
        static syntax => syntax.WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));
}