using System.Linq;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// Decorator to check <see cref="ITypeSymbol"/> recursively.
/// </summary>
internal class RecursiveTypeSymbolChecker : TypeChecker
{
    private readonly TypeChecker _inner;

    /// <summary>
    /// Creates new instance of <see cref="RecursiveTypeSymbolChecker"/>.
    /// </summary>
    /// <param name="inner">Inner checker.</param>
    public RecursiveTypeSymbolChecker(TypeChecker inner) { _inner = inner; }

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
