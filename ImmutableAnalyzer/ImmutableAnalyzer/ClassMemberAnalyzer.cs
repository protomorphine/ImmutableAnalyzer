using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer;

/// <summary>
/// Base class for member analyzers in immutable class.
/// <typeparam name="TMemberSyntax">Type of class member to analyze.</typeparam>
/// </summary>
internal abstract class ClassMemberAnalyzer<TMemberSyntax> : DiagnosticAnalyzer
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
    protected abstract void AnalyzeSyntax(TMemberSyntax node, SyntaxNodeAnalysisContext context);

    /// <summary>
    /// Execute analyzer.
    /// Executes <see cref="AnalyzeSyntax(TMemberSyntax, SyntaxNodeAnalysisContext)"/> only if class declaration
    /// marked by <see cref="ImmutableAttribute"/>.
    /// </summary>
    /// <param name="context">Analysis context.</param>
    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not TypeDeclarationSyntax typeDeclaration || !typeDeclaration.HasAttribute<ImmutableAttribute>())
            return;

        foreach (var member in typeDeclaration.Members)
        {
            if (member is TMemberSyntax syntaxNode)
                AnalyzeSyntax(syntaxNode, context);
        }
    }
}