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
internal class ImmutablePropertyTypeAnalyzer : BaseImmutableAnalyzer
{
    protected override string DiagnosticId => "IM0001";
    protected override string Title => "Mutable member in immutable class";
    protected override string MessageFormat => "Immutable class can't have property of type '{0}'";
    protected override string Description => "Class member must have immutable type";
    protected override string Category => "Design";

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
            GenericNameSyntax => Constants.ImmutableGenericClassTypes.Contains(symbol.MetadataName),
            _ => Constants.ImmutableClassTypes.Contains(symbol.Name)
        };
    }
}