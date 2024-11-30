using ImmutableAnalyzer.Extensions;
using ImmutableAnalyzer.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer;

/// <summary>
/// Base type for immutability analysis.
/// </summary>
internal abstract class ImmutableAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Checks if analysis node should be checked. 
    /// </summary>
    /// <param name="context">Analysis context.</param>
    /// <returns>true - if node should be checked, otherwise - false.</returns>
    protected static bool IsAnalysisNodeMarkedImmutable(SyntaxNodeAnalysisContext context)
    {
        if (context.ContainingSymbol is null)
            return false;

        return context
            .Compilation
            .HasAttribute(context.ContainingSymbol, "ImmutableAnalyzer.Attributes.ImmutableAttribute");
    }

    /// <summary>
    /// Checks <paramref name="type"/> for immutability.
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <param name="context">Analysis context.</param>
    /// <returns>true - if given type is immutable, otherwise - false.</returns>
    protected static bool IsImmutable(TypeSyntax type, SyntaxNodeAnalysisContext context)
    {
        var checker = ImmutableTypeCheckerService.Create(context);
        return checker.IsImmutable(context.SemanticModel.GetTypeSymbolFromSyntaxNode(type));
    }
}
