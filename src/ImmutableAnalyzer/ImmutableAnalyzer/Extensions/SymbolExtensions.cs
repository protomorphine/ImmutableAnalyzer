using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Extensions;

/// <summary>
/// Extensions for <see cref="ISymbol"/>
/// </summary>
internal static class SymbolExtensions
{
    /// <summary>
    /// Checks if <paramref name="symbol"/> marked by given attribute.
    /// </summary>
    /// <param name="symbol">Symbol, exposed by compiler.</param>
    /// <returns>true - if <paramref name="symbol"/> marked by given attribute, otherwise - false.</returns>
    public static bool HasAttribute<T>(this ISymbol symbol) =>
        symbol.GetAttributes().Any(data => data.AttributeClass?.Name == typeof(T).Name);

    /// <summary>
    /// Returns symbol from type.
    /// </summary>
    /// <param name="type">Type syntax expression.</param>
    /// <param name="semanticModel">Semantic model.</param>
    /// <returns><see cref="ITypeSymbol"/> of given type.</returns>
    public static ITypeSymbol GetTypeSymbolFromSyntaxNode(this SemanticModel semanticModel, SyntaxNode type)
    {
        var symbol = semanticModel.GetTypeInfo(type).Type;

        return symbol switch
        {
            IArrayTypeSymbol arrayTypeSymbol => arrayTypeSymbol.ElementType, // detect arrays e.g. int[] -> int
            not null => symbol,                                              // just return type symbol
            _ => throw new NotSupportedException("Not supported symbol")
        };
    }
}