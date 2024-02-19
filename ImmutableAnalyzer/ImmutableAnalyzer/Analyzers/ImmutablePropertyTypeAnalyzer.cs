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
    private const string DiagnosticId = "IM0001";
    private const string Title = "Mutable member in immutable class";
    private const string MessageFormat = "Immutable class can't have property of type '{0}'";
    private const string Description = "Class member must have immutable type.";
    private const string Category = "Design";
    
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

    /// <summary>
    /// Diagnostic descriptor.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId, Title, MessageFormat,
        Category, DiagnosticSeverity.Error, true, Description
    );

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