namespace ImmutableAnalyzer.CodeFixes.SetAccessor.Strategies;

/// <summary>
/// Removes 'set' accessor.
/// </summary>
internal class RemoveSetAccessor : ChangeSetAccessorCodeFix
{
    /// <inheritdoc />
    public override string GetTitle(string format) => "Remove set accessor";

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } = (node, editor) => editor.RemoveNode(node);
}
