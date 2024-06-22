using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Abstractions;

/// <summary>
/// Base class for type declaration analyzer.
/// </summary>
/// <typeparam name="TDeclarationSyntax">Type of type declaration syntax.</typeparam>
internal abstract class TypeDeclarationAnalyzer<TDeclarationSyntax> : DiagnosticAnalyzer
    where TDeclarationSyntax : TypeDeclarationSyntax
{
    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(
            AnalyzeSyntax,
            SyntaxKind.ClassDeclaration, SyntaxKind.RecordDeclaration,
            SyntaxKind.StructDeclaration, SyntaxKind.InterfaceDeclaration
        );
    }

    /// <summary>
    /// Executes analyzer.
    /// </summary>
    /// <param name="node">Type declaration syntax node.</param>
    /// <param name="ctx">Syntax node analysis context.</param>
    protected abstract void AnalyzeTypeDeclaration(TDeclarationSyntax node, SyntaxNodeAnalysisContext ctx);

    /// <summary>
    /// Check's if context node marked by <see cref="ImmutableAttribute"/> and executes analyzer.
    /// </summary>
    /// <param name="context">Syntax node analysis context.</param>
    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.ContainingSymbol is { } symbol && !symbol.HasAttribute<ImmutableAttribute>())
            return;

        if (context.Node is TDeclarationSyntax typeDeclarationSyntax)
            AnalyzeTypeDeclaration(typeDeclarationSyntax, context);
    }
}
