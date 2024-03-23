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
        var symbol = context.SemanticModel.GetSymbolInfo(node.Type).Symbol;
        if (symbol is null || IsImmutable((INamedTypeSymbol) symbol))
            return;

        var diagnostic = Diagnostic.Create(
            Rule, node.Type.GetLocation(),
            node.Type.ToFullString().Trim()
        );
        context.ReportDiagnostic(diagnostic);
    }

    private static bool IsImmutable(INamedTypeSymbol symbol)
    {
        if (symbol.IsUserDefinedImmutable())
            return true;

        if (ImmutableClassTypes.Contains(symbol.Name) || ImmutableGenericClassTypes.Contains(symbol.MetadataName))
            return true;

        if (symbol.BaseType is { } baseType)
            // ReSharper disable once TailRecursiveCall
            return IsImmutable(baseType);

        return symbol.AllInterfaces.Any(IsImmutable);
    }
}