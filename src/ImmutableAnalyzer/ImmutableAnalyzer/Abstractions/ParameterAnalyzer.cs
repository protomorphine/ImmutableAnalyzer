using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Abstractions;

/// <summary>
/// Base class for parameter analyzer in immutable records.
/// </summary>
internal abstract class ParameterAnalyzer : TypeDeclarationAnalyzer<RecordDeclarationSyntax>
{
    /// <summary>
    /// Executes analyzer.
    /// </summary>
    /// <param name="node">Parameter syntax node.</param>
    /// <param name="ctx">Syntax node analysis context.</param>
    protected abstract void AnalyzeParameter(ParameterSyntax node, SyntaxNodeAnalysisContext ctx);

    /// <inheritdoc />
    protected override void AnalyzeTypeDeclaration(RecordDeclarationSyntax node, SyntaxNodeAnalysisContext ctx)
    {
        if (node.ParameterList is null)
            return;

        foreach (var parameter in node.ParameterList.Parameters)
            AnalyzeParameter(parameter, ctx);
    }
}
