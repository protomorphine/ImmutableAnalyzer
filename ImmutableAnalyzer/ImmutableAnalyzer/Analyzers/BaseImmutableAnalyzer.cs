using System.Collections.Immutable;
using System.Linq;
using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Analyzers;

internal abstract class BaseImmutableAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic Identifier.
    /// </summary>
    protected abstract string DiagnosticId { get; }

    /// <summary>
    /// Diagnostic title.
    /// </summary>
    protected abstract string Title { get; }

    /// <summary>
    /// Diagnostic description.
    /// </summary>
    protected abstract string Description { get; }

    /// <summary>
    /// Diagnostic message format.
    /// </summary>
    protected abstract string MessageFormat { get; }

    /// <summary>
    /// Diagnostic category.
    /// </summary>
    protected abstract string Category { get; }

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Diagnostic descriptor.
    /// </summary>
    protected DiagnosticDescriptor Rule => new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error,
        isEnabledByDefault: true, description: Description);

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ClassDeclaration);
    }

    /// <summary>
    /// Execute analyzer.
    /// </summary>
    /// <param name="classDeclarationNode">Class syntax node.</param>
    /// <param name="context">Analysis context.</param>
    protected abstract void AnalyzeSyntax(ClassDeclarationSyntax classDeclarationNode, SyntaxNodeAnalysisContext context);

    /// <summary>
    /// Execute analyzer.
    /// </summary>
    /// <param name="context">Analysis context.</param>
    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclarationNode ||
            !classDeclarationNode.HasAttribute<ImmutableAttribute>())
            return;

        AnalyzeSyntax(classDeclarationNode, context);
    }
}