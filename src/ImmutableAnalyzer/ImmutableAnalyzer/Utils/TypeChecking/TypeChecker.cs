using System;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// <see cref="ITypeSymbol"/> immutability checker.
/// </summary>
internal abstract class TypeChecker
{
    /// <summary>
    /// Checks <paramref name="typeSymbol"/> for immutability.
    /// </summary>
    /// <param name="typeSymbol">Type symbol.</param>
    /// <returns>true - if <paramref name="typeSymbol"/> is immutable, otherwise - false.</returns>
    public abstract bool IsImmutable(ITypeSymbol typeSymbol);
}