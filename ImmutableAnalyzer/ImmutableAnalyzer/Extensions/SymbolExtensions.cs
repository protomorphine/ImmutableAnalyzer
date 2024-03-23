using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImmutableAnalyzer.Extensions;

/// <summary>
/// Extensions for <see cref="ISymbol"/>
/// </summary>
public static class SymbolExtensions
{
    /// <summary>
    /// Checks if <see cref="symbol"/> has <see cref="ImmutableAttribute"/> in any declaring syntax references.
    /// </summary>
    /// <param name="symbol">Symbol, exposed by compiler.</param>
    /// <returns>true - if symbol has <see cref="ImmutableAttribute"/>, otherwise - false.</returns>
    public static bool IsUserDefinedImmutable(this ISymbol symbol)
    {
        foreach (var syntaxReference in symbol.DeclaringSyntaxReferences)
        {
            if (syntaxReference.GetSyntax() is not TypeDeclarationSyntax typeDeclarationSyntax)
                continue;

            if (typeDeclarationSyntax.HasAttribute<ImmutableAttribute>())
                return true;
        }

        return false;
    }
}