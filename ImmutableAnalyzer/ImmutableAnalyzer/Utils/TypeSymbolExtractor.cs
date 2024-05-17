using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Utils;

internal struct TypeSymbolExtractor
{
    /// <summary>
    /// Returns symbol from type and context.
    /// </summary>
    /// <param name="type">Type syntax expression.</param>
    /// <param name="ctx">Syntax node analysis context.</param>
    /// <returns><see cref="ISymbol"/> of given type.</returns>
    /// <exception cref="ArgumentNullException">Throws when type is null.</exception>
    /// <exception cref="InvalidOperationException">Throws when semantic model can't provide information about type.</exception>
    public static ITypeSymbol GetSymbolFromSyntaxNode(SyntaxNode type, SyntaxNodeAnalysisContext ctx)
    {
        var symbol = ctx.SemanticModel.GetSymbolInfo(type).Symbol ??
                     throw new InvalidOperationException("Semantic model doesn't provide information about symbol");

        return symbol switch
        {
            IArrayTypeSymbol arrayTypeSymbol => arrayTypeSymbol.ElementType,
            ITypeSymbol typeSymbol => typeSymbol,
            _ => throw new NotSupportedException("Not supported symbol")
        };
    }
}