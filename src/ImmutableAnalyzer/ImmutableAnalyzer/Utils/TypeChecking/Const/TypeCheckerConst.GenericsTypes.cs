using System.Collections.Generic;
using System.Collections.Immutable;

namespace ImmutableAnalyzer.Utils.TypeChecking.Const;

internal partial struct TypeCheckerConst
{
    /// <summary>
    /// Set of generic immutable class types.
    /// </summary>
    public static readonly ImmutableArray<string> ImmutableGenericTypes = ImmutableArray.Create(
        typeof(ImmutableArray<>).Name, typeof(ImmutableDictionary<,>).Name, typeof(ImmutableList<>).Name,
        typeof(ImmutableHashSet<>).Name, typeof(ImmutableSortedDictionary<,>).Name, typeof(ImmutableSortedSet<>).Name,
        typeof(ImmutableStack<>).Name, typeof(ImmutableQueue<>).Name, typeof(IReadOnlyList<>).Name,
        typeof(IReadOnlyCollection<>).Name, typeof(IReadOnlyDictionary<,>).Name
    );
}