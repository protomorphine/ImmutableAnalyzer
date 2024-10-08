using System.Collections.Immutable;
using ImmutableAnalyzer.Common;
using ImmutableAnalyzer.Extensions;
using ImmutableAnalyzer.Utils.TypeChecking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.ParameterAnalyzers;

/// <summary>
/// Analyzer to check parameter type of records.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class ParameterTypeAnalyzer : ParameterAnalyzer
{
    /// <summary>
    /// Instance of <see cref="TypeChecker"/>.
    /// </summary>
    private static readonly TypeChecker Checker = TypeCheckerFactory.GetOrCreate();

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    /// <summary>
    /// Diagnostic descriptor.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule =
        new(
            id: "IM0003",
            title: "Mutable parameter in record parameter list",
            messageFormat: "Immutable record can't have parameter of type '{0}'",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Record parameter must have immutable type."
        );

    /// <inheritdoc />
    protected override void AnalyzeParameter(ParameterSyntax node, SyntaxNodeAnalysisContext ctx)
    {
        if (Checker.IsImmutable(ctx.SemanticModel.GetTypeSymbolFromSyntaxNode(node.Type)))
            return;

        var diagnostic = Diagnostic.Create(
            Rule,
            node.Type!.GetLocation(),
            node.Type.ToFullString().Trim()
        );

        ctx.ReportDiagnostic(diagnostic);
    }
}
