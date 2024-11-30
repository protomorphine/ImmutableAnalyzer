namespace ImmutableAnalyzer.Attributes;

/// <summary>
/// Represent immutable type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public class ImmutableAttribute : Attribute
{ }
