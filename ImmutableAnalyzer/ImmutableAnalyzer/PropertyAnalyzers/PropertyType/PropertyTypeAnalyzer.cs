﻿using System.Collections.Immutable;
using ImmutableAnalyzer.Utils.TypeChecking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.PropertyAnalyzers.PropertyType;

/// <summary>
/// Analyzer to check properties type of immutable types.
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
        title:              "Mutable property in immutable type",
        messageFormat:      "Immutable type can't have property of type '{0}'",
        category:           "Design",
        defaultSeverity:    DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description:        "Class member must have immutable type."
    );

    /// <inheritdoc/>
    protected override void AnalyzeMember(PropertyDeclarationSyntax node, SyntaxNodeAnalysisContext ctx)
    {
        if (node is { Type: not null } && TypeChecker.IsImmutable(node.Type, ctx))
            return;

        var diagnostic = Diagnostic.Create(
            Rule, node.Type!.GetLocation(),
            node.Type.ToFullString().Trim()
        );

        ctx.ReportDiagnostic(diagnostic);
    }
}