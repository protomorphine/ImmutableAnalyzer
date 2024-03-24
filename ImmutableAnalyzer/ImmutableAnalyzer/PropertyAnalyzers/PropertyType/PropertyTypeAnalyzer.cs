using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.PropertyAnalyzers.PropertyType;

/// <summary>
/// Analyzer to check property types of immutable classes.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class PropertyTypeAnalyzer : PropertyAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

    /// <summary>
    /// Diagnostic descriptor.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        id:                 "IM0001",
        title:              "Mutable member in immutable class",
        messageFormat:      "Immutable class can't have property of type '{0}'",
        category:           "Design",
        defaultSeverity:    DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description:        "Class member must have immutable type."
    );

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

    /// <inheritdoc/>
    protected override void AnalyzeSyntax(PropertyDeclarationSyntax node, SyntaxNodeAnalysisContext context)
    {
        if (context.SemanticModel.GetSymbolInfo(node.Type).Symbol is not INamedTypeSymbol symbol || IsImmutable(symbol))
            return;

        var diagnostic = Diagnostic.Create(
            Rule, node.Type.GetLocation(),
            node.Type.ToFullString().Trim()
        );
        context.ReportDiagnostic(diagnostic);
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
    private static bool IsImmutable(INamedTypeSymbol symbol)
    {
        if (symbol.IsUserDefinedImmutable()                          ||
            ImmutableClassTypes.Contains(symbol.Name)                ||
            ImmutableGenericClassTypes.Contains(symbol.MetadataName)
        )
        {
            return true;
        }

        // ReSharper disable once TailRecursiveCall
        return symbol.BaseType is { } baseType ? IsImmutable(baseType) : symbol.AllInterfaces.Any(IsImmutable);
    }
}