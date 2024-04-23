using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer;

/// <summary>
/// Provides helper methods to check types for immutability.
/// </summary>
internal struct TypeChecker
{
    /// <summary>
    /// Set of valid immutable class types.
    /// </summary>
    public static readonly ImmutableArray<string> ImmutableClassTypes = ImmutableArray.Create(
        nameof(Boolean), nameof(Byte), nameof(SByte), nameof(Char), nameof(Decimal), nameof(Double), nameof(Single),
        nameof(Int32), nameof(UInt32), nameof(Int64), nameof(UInt64), nameof(Int16), nameof(UInt16), nameof(String),
        nameof(DateTime), nameof(Guid), nameof(Enum)
    );

    /// <summary>
    /// Set of generic immutable class types.
    /// </summary>
    public static readonly ImmutableArray<string> ImmutableGenericClassTypes = ImmutableArray.Create(
        typeof(ImmutableArray<>).Name, typeof(ImmutableDictionary<,>).Name, typeof(ImmutableList<>).Name,
        typeof(ImmutableHashSet<>).Name, typeof(ImmutableSortedDictionary<,>).Name, typeof(ImmutableSortedSet<>).Name,
        typeof(ImmutableStack<>).Name, typeof(ImmutableQueue<>).Name, typeof(IReadOnlyList<>).Name,
        typeof(IReadOnlyCollection<>).Name, typeof(IReadOnlyDictionary<,>).Name
    );

    /// <summary>
    /// Check's <see cref="ParameterSyntax"/> type for immutability.
    /// </summary>
    /// <param name="node">Node for check.</param>
    /// <param name="ctx">Syntax node analysis context.</param>
    /// <returns>true - if node type is immutable, otherwise - false.</returns>
    public static bool IsImmutable(ParameterSyntax node, SyntaxNodeAnalysisContext ctx) =>
        IsImmutable(GetSymbolFromType(node.Type, ctx));

    /// <summary>
    /// Check's <see cref="PropertyDeclarationSyntax"/> type for immutability.
    /// </summary>
    /// <param name="node">Node for check.</param>
    /// <param name="ctx">Syntax node analysis context.</param>
    /// <returns>true - if node type is immutable, otherwise - false.</returns>
    public static bool IsImmutable(PropertyDeclarationSyntax node, SyntaxNodeAnalysisContext ctx) =>
        IsImmutable(GetSymbolFromType(node.Type, ctx));

    /// <summary>
    /// Returns symbol from type and context.
    /// </summary>
    /// <param name="type">Type syntax expression.</param>
    /// <param name="ctx">Syntax node analysis context.</param>
    /// <returns><see cref="ISymbol"/> of given type.</returns>
    /// <exception cref="ArgumentNullException">Throws when type is null.</exception>
    /// <exception cref="InvalidOperationException">Throws when semantic model can't provide information about type.</exception>
    private static ITypeSymbol GetSymbolFromType(TypeSyntax? type, SyntaxNodeAnalysisContext ctx)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type), "Given parameter syntax node doesn't contain type");

        var symbol = ctx.SemanticModel.GetSymbolInfo(type).Symbol ??
                     throw new InvalidOperationException("Semantic model doesn't provide information about symbol");

        return symbol switch
        {
            IArrayTypeSymbol arrayTypeSymbol => arrayTypeSymbol.ElementType,
            ITypeSymbol typeSymbol => typeSymbol,
            _ => throw new NotSupportedException("Not supported symbol")
        };
    }

    /// <summary>
    /// Checks given symbol for immutability.
    /// <br />
    /// Algorithm:
    /// 1. Check if symbol underlying type marked by <see cref="ImmutableAttribute"/>; <br />
    /// 2. Check if <see cref="ImmutableClassTypes"/> or <see cref="ImmutableGenericClassTypes"/> contains symbol name; <br />
    /// 3. If 1 - 2 steps gives false - checks if symbol has a base type to define is we working with interface or not. <br />
    /// 3.1. If we working with type (not interface) - recursively check symbol base type. <br />
    /// 3.2. If we working with interface - recursively check all interfaces. <br />
    /// </summary>
    /// <param name="symbol">Symbol.</param>
    /// <returns>true - if symbol is immutable, otherwise - false.</returns>
    private static bool IsImmutable(ITypeSymbol symbol)
    {
        if (symbol.IsUserDefinedImmutable()
            || ImmutableClassTypes.Contains(symbol.Name)
            || ImmutableGenericClassTypes.Contains(symbol.MetadataName))
        {
            return true;
        }

        // ReSharper disable once TailRecursiveCall
        return symbol.BaseType is { } baseType ? IsImmutable(baseType) : symbol.AllInterfaces.Any(IsImmutable);
    }
}