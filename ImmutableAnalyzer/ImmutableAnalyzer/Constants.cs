using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ImmutableAnalyzer;

/// <summary>
/// Immutable analyzers constants.
/// </summary>
internal struct Constants
{
    /// <summary>
    /// Set of valid immutable class types.
    /// </summary>
    public static readonly IReadOnlySet<string> ImmutableClassTypes = new HashSet<string>
    {
        nameof(Boolean), nameof(Byte), nameof(SByte), nameof(Char), nameof(Decimal), nameof(Double), nameof(Single),
        nameof(Int32), nameof(UInt32), nameof(Int64), nameof(UInt64), nameof(Int16), nameof(UInt16), nameof(String)
    };

    /// <summary>
    /// Set of generic immutable class types.
    /// </summary>
    public static readonly IReadOnlySet<string> ImmutableGenericClassTypes = new HashSet<string>
    {
        typeof(ImmutableArray<>).Name, typeof(ImmutableDictionary<,>).Name, typeof(ImmutableList<>).Name,
        typeof(ImmutableHashSet<>).Name, typeof(ImmutableSortedDictionary<,>).Name, typeof(ImmutableSortedSet<>).Name,
        typeof(ImmutableStack<>).Name, typeof(ImmutableQueue<>).Name, typeof(IReadOnlyList<>).Name,
        typeof(IReadOnlySet<>).Name, typeof(IReadOnlyCollection<>).Name, typeof(IReadOnlyDictionary<,>).Name
    };
}