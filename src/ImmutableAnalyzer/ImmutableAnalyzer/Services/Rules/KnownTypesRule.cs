using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Services.Rules;

/// <summary>
/// Checks <see cref="ITypeSymbol"/> for immutability by declared types.
/// </summary>
internal class KnownTypesRule : IImmutabilityCheckRule
{
    /// <summary>
    /// Set of generic immutable class types.
    /// </summary>
    private static readonly ImmutableArray<string> GenericTypes = ImmutableArray.Create(
        typeof(ImmutableArray<>).Name, typeof(ImmutableDictionary<,>).Name, typeof(ImmutableList<>).Name,
        typeof(ImmutableHashSet<>).Name, typeof(ImmutableSortedDictionary<,>).Name, typeof(ImmutableSortedSet<>).Name,
        typeof(ImmutableStack<>).Name, typeof(ImmutableQueue<>).Name, typeof(IReadOnlyList<>).Name,
        typeof(IReadOnlyCollection<>).Name, typeof(IReadOnlyDictionary<,>).Name
    );

    /// <summary>
    /// Set of valid immutable types.
    /// </summary>
    private static readonly ImmutableArray<string> BasicTypes = ImmutableArray.Create(
        nameof(Boolean), nameof(Byte), nameof(SByte), nameof(Char), nameof(Decimal), nameof(Double), nameof(Single),
        nameof(Int32), nameof(UInt32), nameof(Int64), nameof(UInt64), nameof(Int16), nameof(UInt16), nameof(String),
        nameof(DateTime), nameof(Guid), nameof(Enum)
    );

    /// <inheritdoc />
    public bool IsImmutable(ITypeSymbol typeSymbol) =>
        BasicTypes.Contains(typeSymbol.Name) || GenericTypes.Contains(typeSymbol.MetadataName);
}
