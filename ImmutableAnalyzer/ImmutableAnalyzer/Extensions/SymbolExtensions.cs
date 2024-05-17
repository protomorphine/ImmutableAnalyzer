using System.Linq;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Extensions;

/// <summary>
/// Extensions for <see cref="ISymbol"/>
/// </summary>
internal static class SymbolExtensions
{
    /// <summary>
    /// Checks if <see cref="symbol"/> marked by given attribute.
    /// </summary>
    /// <param name="symbol">Symbol, exposed by compiler.</param>
    /// <returns>true - if symbol marked by given attribute, otherwise - false.</returns>
    public static bool HasAttribute<T>(this ISymbol symbol) =>
        symbol.GetAttributes().Any(data => data.AttributeClass?.Name == typeof(T).Name);
}