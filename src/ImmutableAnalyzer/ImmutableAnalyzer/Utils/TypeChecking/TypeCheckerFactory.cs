using System;
using System.Threading;

namespace ImmutableAnalyzer.Utils.TypeChecking;

/// <summary>
/// Static factory for <see cref="TypeChecker"/>.
/// </summary>
internal static class TypeCheckerFactory
{
    private static readonly Lazy<TypeChecker> CheckerLazy = new(Create, LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>
    /// Get or creates instance of <see cref="TypeChecker"/>.
    /// </summary>
    /// <returns>Instance of checker.</returns>
    public static TypeChecker GetOrCreate() => CheckerLazy.Value;

    /// <summary>
    /// Creates the instance of checker.
    /// </summary>
    /// <returns>Checker.</returns>
    private static TypeChecker Create()
    {
        var declaredTypesChecker = new DeclaredTypeSymbolChecker();

        return new RecursiveTypeSymbolChecker(declaredTypesChecker);
    }
}