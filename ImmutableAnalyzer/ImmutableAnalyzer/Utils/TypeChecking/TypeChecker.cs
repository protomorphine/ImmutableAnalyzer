using System.Linq;
using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// Provides helper methods to check types for immutability.
/// </summary>
internal partial struct TypeChecker
{
    /// <summary>
    /// Check's <see cref="ParameterSyntax"/> type for immutability.
    /// </summary>
    /// <param name="type">Node for check.</param>
    /// <param name="ctx">Syntax node analysis context.</param>
    /// <returns>true - if node type is immutable, otherwise - false.</returns>
    public static bool IsImmutable(TypeSyntax type, SyntaxNodeAnalysisContext ctx) =>
        IsImmutableInternal(TypeSymbolExtractor.GetSymbolFromSyntaxNode(type, ctx));

    /// <summary>
    /// Checks given symbol for immutability.
    /// <br />
    /// Algorithm:
    /// 1. Check if symbol underlying type marked by <see cref="ImmutableAttribute"/>; <br />
    /// 2. Check if <see cref="ImmutableTypes"/> or <see cref="ImmutableGenericTypes"/> contains symbol name; <br />
    /// 3. If 1 - 2 steps gives false - checks if symbol has a base type to define is we working with interface or not. <br />
    /// 3.1. If we working with type (not interface) - recursively check symbol base type. <br />
    /// 3.2. If we working with interface - recursively check all interfaces. <br />
    /// </summary>
    /// <param name="symbol">Symbol.</param>
    /// <returns>true - if symbol is immutable, otherwise - false.</returns>
    private static bool IsImmutableInternal(ITypeSymbol symbol)
    {
        if (symbol.HasAttribute<ImmutableAttribute>()
            || ImmutableTypes.Contains(symbol.Name)
            || ImmutableGenericTypes.Contains(symbol.MetadataName))
        {
            return true;
        }

        // ReSharper disable once TailRecursiveCall
        return symbol.BaseType is { } baseType ? IsImmutableInternal(baseType) : symbol.AllInterfaces.Any(IsImmutableInternal);
    }
}