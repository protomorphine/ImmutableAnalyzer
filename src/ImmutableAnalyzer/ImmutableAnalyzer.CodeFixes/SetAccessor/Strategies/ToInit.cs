using Microsoft.CodeAnalysis.CSharp;

namespace ImmutableAnalyzer.CodeFixes.SetAccessor.Strategies;

/// <summary>
/// Changes 'set' accessor to 'init'.
/// </summary>
internal class ToInit : ChangeSetAccessorCodeFix
{
    /// <inheritdoc />
    public override string GetTitle(string format) => string.Format(format, "init");

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } = (node, editor) => editor.ReplaceNode(node, node.WithKeyword(SyntaxFactory.Token(SyntaxKind.InitKeyword)));
}
