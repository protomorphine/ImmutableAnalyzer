using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Abstractions;

/// <summary>
/// Base class for member analyzers in immutable class.
/// <typeparam name="TMemberSyntax">Type of class member to analyze.</typeparam>
/// </summary>
internal abstract class MemberAnalyzer<TMemberSyntax> : TypeDeclarationAnalyzer<TypeDeclarationSyntax>
{
    /// <summary>
    /// Execute analyzer.
    /// </summary>
    /// <param name="node">Syntax node.</param>
    /// <param name="ctx">Analysis context.</param>
    protected abstract void AnalyzeMember(TMemberSyntax node, SyntaxNodeAnalysisContext ctx);

    /// <summary>
    /// Execute analyzer.
    /// Executes <see cref="AnalyzeMember"/> only if class declaration
    /// marked by <see cref="ImmutableAttribute"/>.
    /// </summary>
    /// <param name="typeDeclaration">Type declaration node.</param>
    /// <param name="ctx">Analysis context.</param>
    protected override void AnalyzeTypeDeclaration(TypeDeclarationSyntax typeDeclaration, SyntaxNodeAnalysisContext ctx)
    {
        foreach (var member in typeDeclaration.Members)
        {
            if (member is TMemberSyntax syntaxNode)
                AnalyzeMember(syntaxNode, ctx);
        }
    }
}