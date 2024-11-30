using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer;

/// <summary>
/// Analyzer to check parameter type of records.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class ParameterTypeAnalyzer : ImmutableAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

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
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(
            (ctx) =>
            {
                if (!IsAnalysisNodeMarkedImmutable(ctx))
                    return;

                var syntax = (RecordDeclarationSyntax)ctx.Node;
                var parameters = syntax.ParameterList?.Parameters;

                if (parameters is null)
                    return;

                foreach (var member in parameters)
                    AnalyzeParameter(member, ctx);
            },
            SyntaxKind.RecordDeclaration, SyntaxKind.RecordStructDeclaration
        );
    }

    private static void AnalyzeParameter(ParameterSyntax parameter, SyntaxNodeAnalysisContext ctx)
    {
        if (parameter.Type is not { } parameterType)
            return;

        if (IsImmutable(parameterType, ctx))
            return;

        var diagnostic = Diagnostic.Create(
            Rule,
            parameterType.GetLocation(),
            parameterType.ToFullString().Trim()
        );

        ctx.ReportDiagnostic(diagnostic);
    }
}
