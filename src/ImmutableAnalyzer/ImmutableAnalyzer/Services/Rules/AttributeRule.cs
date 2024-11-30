using System.Linq;
using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Services.Rules;

/// <summary>
/// Checks <see cref="ITypeSymbol"/> by <see cref="ImmutableAttribute"/> on it.
/// </summary>
internal class AttributeRule : IImmutabilityCheckRule
{
    private readonly Compilation _compilation;

    /// <summary>
    /// Creates new instance of <see cref="AttributeRule"/>.
    /// </summary>
    /// <param name="analysisContext">Context.</param>
    public AttributeRule(SyntaxNodeAnalysisContext analysisContext)
    {
        _compilation = analysisContext.Compilation;
    }

    /// <inheritdoc/>
    public bool IsImmutable(ITypeSymbol typeSymbol) =>
        _compilation.HasAttribute(typeSymbol, "ImmutableAnalyzer.Attributes.ImmutableAttribute");
}
