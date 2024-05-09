using Microsoft.CodeAnalysis.CSharp;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixStrategies;

/// <summary>
/// Changes 'set' accessor to 'init'.
/// </summary>
internal class ToInitStrategy : ChangeSetAccessorStrategy
{
    /// <inheritdoc />
    public override string GetTitle(string format) => string.Format(format, "init");

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } =
        static syntax => syntax.WithKeyword(SyntaxFactory.Token(SyntaxKind.InitKeyword));
}