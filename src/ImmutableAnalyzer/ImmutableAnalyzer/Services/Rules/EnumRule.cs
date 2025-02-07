using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Services.Rules;

/// <summary>
/// Checks if given <see cref="ITypeSymbol"/> is enmum.
/// </summary>
public class EnumRule : IImmutabilityCheckRule
{
    /// <inheritdoc/>
    public bool IsImmutable(ITypeSymbol typeSymbol) => typeSymbol.TypeKind == TypeKind.Enum;
}
