using ImmutableAnalyzer.Extensions;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// 
/// </summary>
internal class DeclaredTypeSymbolChecker : TypeChecker
{
    private readonly TypeChecker? _inner;

    public DeclaredTypeSymbolChecker(TypeChecker? inner = null)
    {
        _inner = inner;
    }

    public override bool IsImmutable(ITypeSymbol typeSymbol)
    {
        var isImmutableFromInner = _inner?.IsImmutable(typeSymbol);

        if (isImmutableFromInner.HasValue && !isImmutableFromInner.Value)
            return false;

        return (isImmutableFromInner ?? true) && typeSymbol.HasAttribute<ImmutableAttribute>() ||
            Const.TypeCheckerConst.ImmutableTypes.Contains(typeSymbol.Name) ||
            Const.TypeCheckerConst.ImmutableGenericTypes.Contains(typeSymbol.MetadataName);
    }
}