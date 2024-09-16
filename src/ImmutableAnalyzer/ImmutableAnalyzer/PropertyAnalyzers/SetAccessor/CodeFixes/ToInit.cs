using Microsoft.CodeAnalysis.CSharp;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixes;

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