using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Analyzers;

internal abstract class BaseImmutableAnalyzer : DiagnosticAnalyzer
{
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