using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Analyzers;

/// <summary>
/// Base class for immutable analyzers.
/// <typeparam name="TSyntax">Type of class member to analyze.</typeparam>
/// </summary>
internal abstract class ClassMemberAnalyzer<TSyntax> : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(
            AnalyzeSyntax,
            SyntaxKind.ClassDeclaration, SyntaxKind.RecordDeclaration, SyntaxKind.StructDeclaration
        );
    }

    /// <summary>
    /// Execute analyzer.
    /// </summary>
    /// <param name="node">Syntax node.</param>
    /// <param name="context">Analysis context.</param>
    protected abstract void AnalyzeSyntax(TSyntax node, SyntaxNodeAnalysisContext context);

    /// <summary>
    /// Execute analyzer.
    /// Executes <see cref="AnalyzeSyntax(TSyntax, SyntaxNodeAnalysisContext)"/> only if class declaration
    /// marked by <see cref="ImmutableAttribute"/>.
    /// </summary>
    /// <param name="context">Analysis context.</param>
    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration
            || !classDeclaration.HasAttribute<ImmutableAttribute>())
            return;

        foreach (var member in classDeclaration.Members)
        {
            if (member is TSyntax syntaxNode)
                AnalyzeSyntax(syntaxNode, context);
        }
    }
}