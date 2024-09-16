using System.Linq;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// Decorator to check <see cref="ITypeSymbol"/> recursively.
/// </summary>
internal class RecursiveTypeSymbolChecker(TypeChecker inner) : TypeChecker
{
    private readonly TypeChecker _inner = inner;

    /// <inheritdoc />
    public override bool IsImmutable(ITypeSymbol typeSymbol)
    {
        if (_inner.IsImmutable(typeSymbol))
            return true;

        return typeSymbol.BaseType is not null
            ? IsImmutable(typeSymbol.BaseType)
            : typeSymbol.AllInterfaces.Any(IsImmutable);
    }
}