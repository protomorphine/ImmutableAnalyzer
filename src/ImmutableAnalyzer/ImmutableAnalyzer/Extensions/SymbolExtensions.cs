using System;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Extensions;

/// <summary>
/// Extensions for <see cref="ISymbol"/>
/// </summary>
internal static class SymbolExtensions
{
    /// <summary>
    /// Returns symbol from type.
    /// </summary>
    /// <param name="type">Type syntax expression.</param>
    /// <param name="semanticModel">Semantic model.</param>
    /// <returns><see cref="ITypeSymbol"/> of given type.</returns>
    public static ITypeSymbol GetTypeSymbolFromSyntaxNode(this SemanticModel semanticModel, SyntaxNode? type)
    {
        var symbol = type is not null ? semanticModel.GetTypeInfo(type).Type : null;

        return symbol switch
        {
            IArrayTypeSymbol arrayTypeSymbol => arrayTypeSymbol.ElementType, // detect arrays e.g. int[] -> int
            not null => symbol,                      // just return type symbol
            _ => throw new NotSupportedException("Not supported symbol")
        };
    }
}
