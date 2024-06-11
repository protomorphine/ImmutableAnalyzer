namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixes;

/// <summary>
/// Removes 'set' accessor.
/// </summary>
internal class RemoveSetAccessor : ChangeSetAccessorStrategy
{
    /// <inheritdoc />
    public override string GetTitle(string format) => "Remove set accessor";

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } = static _ => null;
}