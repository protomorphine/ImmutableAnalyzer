namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixStrategies;

/// <summary>
/// Removes 'set' accessor.
/// </summary>
internal class RemoveSetAccessorStrategy : ChangeSetAccessorStrategy
{
    /// <inheritdoc />
    public override string GetTitle(string format) => "Remove set accessor";

    /// <inheritdoc />
    protected override AccessorModifier Modifier { get; } = _ => null;
}