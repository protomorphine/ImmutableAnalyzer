using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer;

/// <summary>
/// Analyzer to check properties type of immutable types.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class PropertyTypeAnalyzer : ImmutableAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

    /// <summary>
    /// Diagnostic descriptor.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new(
        id: "IM0001",
        title: "Mutable property in immutable type",
        messageFormat: "Immutable type can't have property of type '{0}'",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Class member must have immutable type."
    );

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(
            (ctx) =>
            {
                if (!IsAnalysisNodeMarkedImmutable(ctx))
                    return;

                var syntax = (TypeDeclarationSyntax)ctx.Node;
                var properties = syntax.Members.OfType<PropertyDeclarationSyntax>();

                foreach (var property in properties)
                    AnalyzeMember(property, ctx);
            },
            SyntaxKind.InterfaceDeclaration,
            SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration,
            SyntaxKind.RecordDeclaration, SyntaxKind.RecordStructDeclaration
        );
    }

    private static void AnalyzeMember(PropertyDeclarationSyntax property, SyntaxNodeAnalysisContext ctx)
    {
        if (property.Type is not { } propertyType)
            return;

        if (IsImmutable(propertyType, ctx))
            return;

        var diagnostic = Diagnostic.Create(
            Rule,
            propertyType.GetLocation(),
            propertyType.ToFullString().Trim()
        );

        ctx.ReportDiagnostic(diagnostic);
    }
}
