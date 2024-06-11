using System.Linq;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// Decorator to check <see cref="ITypeSymbol"/> recursively.
/// </summary>
internal class RecursiveTypeSymbolChecker : TypeChecker
{
    private readonly TypeChecker _inner;

    public RecursiveTypeSymbolChecker(TypeChecker inner)
    {
        _inner = inner;
    }

    /// <inheritdoc />
    public override bool IsImmutable(ITypeSymbol typeSymbol)
    {
        while (true)
        {
            if (_inner.IsImmutable(typeSymbol))
                return true;

            if (typeSymbol.BaseType is not { } baseType)
                return typeSymbol.AllInterfaces.Any(IsImmutable);

            typeSymbol = baseType;
        }
    }
}