using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Analyzers;

/// <summary>
/// Analyzer to check property types of immutable classes.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class ImmutablePropertyTypeAnalyzer : BaseImmutableAnalyzer
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
    public static readonly IReadOnlySet<string> ImmutableClassTypes = new HashSet<string>
    {
        nameof(Boolean), nameof(Byte), nameof(SByte), nameof(Char), nameof(Decimal), nameof(Double), nameof(Single),
        nameof(Int32), nameof(UInt32), nameof(Int64), nameof(UInt64), nameof(Int16), nameof(UInt16), nameof(String)
    };

    /// <summary>
    /// Set of generic immutable class types.
    /// </summary>
    public static readonly IReadOnlySet<string> ImmutableGenericClassTypes = new HashSet<string>
    {
        typeof(ImmutableArray<>).Name, typeof(ImmutableDictionary<,>).Name, typeof(ImmutableList<>).Name,
        typeof(ImmutableHashSet<>).Name, typeof(ImmutableSortedDictionary<,>).Name, typeof(ImmutableSortedSet<>).Name,
        typeof(ImmutableStack<>).Name, typeof(ImmutableQueue<>).Name, typeof(IReadOnlyList<>).Name,
        // TODO: #1 issue
        /*typeof(IReadOnlySet<>).Name,*/ typeof(IReadOnlyCollection<>).Name, typeof(IReadOnlyDictionary<,>).Name
    };

    /// <inheritdoc/>
    protected override void AnalyzeSyntax(
        ClassDeclarationSyntax classDeclarationNode,
        SyntaxNodeAnalysisContext context
    )
    {
        foreach (var member in classDeclarationNode.Members)
        {
            if (member is not PropertyDeclarationSyntax propertyDeclarationNode)
                continue;

            var symbol = context.SemanticModel.GetSymbolInfo(propertyDeclarationNode.Type).Symbol;
            if (symbol is null || IsImmutable(propertyDeclarationNode, symbol))
                continue;

            var diagnostic = Diagnostic.Create(
                Rule, propertyDeclarationNode.Type.GetLocation(),
                propertyDeclarationNode.Type.ToFullString().Trim()
            );
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static bool IsImmutable(BasePropertyDeclarationSyntax prop, ISymbol symbol)
    {
        if (symbol.IsUserDefinedImmutable())
            return true;

        return prop.Type switch
        {
            GenericNameSyntax => ImmutableGenericClassTypes.Contains(symbol.MetadataName),
            _ => ImmutableClassTypes.Contains(symbol.Name)
        };
    }
}