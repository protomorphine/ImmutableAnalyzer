using System.Linq;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Extensions;

/// <summary>
/// Extension methods for <see cref="Compilation"/>.
/// </summary>
internal static class CompilationExtensions
{
    /// <summary>
    /// Checks if <paramref name="symbol"/> marked by given attribute.
    /// Example:
    /// <code>
    /// var hasAttr = context.Compilation.HasAttribute("Test");
    /// </code>
    /// </summary>
    /// <param name="compilation">Compilation.</param>
    /// <param name="symbol">Symbol, exposed by compiler.</param>
    /// <param name="attrMetadataName">Attribute metadata name.</param>
    /// <returns>true - if <paramref name="symbol"/> marked by given attribute, otherwise - false.</returns>
    public static bool HasAttribute(this Compilation compilation, ISymbol symbol, string attrMetadataName)
    {
        var attr = compilation.GetTypeByMetadataName(attrMetadataName);

        return symbol
            .GetAttributes()
            .Any(data => SymbolEqualityComparer.Default.Equals(data.AttributeClass, attr));
    }
}
