using Microsoft.CodeAnalysis;
using ImmutableAnalyzer.Extensions;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// Checks <see cref="ITypeSymbol"/> for immutability by declared types.
/// </summary>
internal class DeclaredTypeSymbolChecker : TypeChecker
{
    private readonly TypeChecker? _inner;

    /// <summary>
    /// Creates new instance of <see cref="RecursiveTypeSymbolChecker"/>.
    /// </summary>
    /// <param name="inner">Inner checker.</param>
    public DeclaredTypeSymbolChecker(TypeChecker? inner = null) { _inner = inner; }

    /// <inheritdoc />
    public override bool IsImmutable(ITypeSymbol typeSymbol)
    {
        if (_inner?.IsImmutable(typeSymbol) == false)
            return false;

        return typeSymbol.HasAttribute<ImmutableAttribute>() ||
            Const.Types.ImmutableTypes.Contains(typeSymbol.Name) ||
            Const.Types.Generic.Contains(typeSymbol.MetadataName);
    }
}
