using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Services.Rules;

/// <summary>
/// Checks <see cref="ITypeSymbol"/> by ImmutableAttribute.
/// </summary>
/// <remarks>
/// Creates new instance of <see cref="AttributeRule"/>.
/// </remarks>
/// <param name="analysisContext">Context.</param>
internal class AttributeRule(SyntaxNodeAnalysisContext analysisContext) : IImmutabilityCheckRule
{
    private readonly Compilation _compilation = analysisContext.Compilation;

    /// <inheritdoc/>
    public bool IsImmutable(ITypeSymbol typeSymbol) =>
        _compilation.HasAttribute(typeSymbol, "ImmutableAnalyzer.Attributes.ImmutableAttribute");
}
