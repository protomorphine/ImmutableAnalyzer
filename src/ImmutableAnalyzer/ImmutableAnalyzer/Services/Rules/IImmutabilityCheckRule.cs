using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Services.Rules;

/// <summary>
/// Represent rule to check type immutability.
/// </summary>
internal interface IImmutabilityCheckRule
{
    /// <summary>
    /// Checks <paramref name="typeSymbol"/> to immutability.
    /// </summary>
    /// <param name="typeSymbol">Type symbol to check.</param>
    /// <returns>true - if <paramref name="typeSymbol"/> is immutable, otherwise - false.</returns>
    public bool IsImmutable(ITypeSymbol typeSymbol);
}
