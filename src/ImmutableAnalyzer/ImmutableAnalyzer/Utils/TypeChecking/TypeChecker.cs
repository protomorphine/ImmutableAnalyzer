using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// <see cref="ITypeSymbol"/> immutability checker.
/// </summary>
internal abstract class TypeChecker
{
    /// <summary>
    /// Checks given type symbol for immutability.
    /// </summary>
    /// <param name="typeSymbol">Type symbol.</param>
    /// <returns>true - if given type symbol is immutable, otherwise - false.</returns>
    public abstract bool IsImmutable(ITypeSymbol typeSymbol);
}
