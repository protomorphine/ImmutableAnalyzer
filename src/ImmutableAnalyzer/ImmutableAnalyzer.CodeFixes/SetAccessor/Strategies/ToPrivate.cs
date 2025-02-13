using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ImmutableAnalyzer.CodeFixes.SetAccessor.Strategies;

/// <summary>
/// Changes access modifier of 'set' accessor to private.
/// </summary>
internal class ToPrivate : ChangeSetAccessorCodeFix
{
    /// <inheritdoc />
    public override string GetTitle(string format) => string.Format(format, "private set");

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } =
        (node, editor) => editor.ReplaceNode(node, node.WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))));
}
