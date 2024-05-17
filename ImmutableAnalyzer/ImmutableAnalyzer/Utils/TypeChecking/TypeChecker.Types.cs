using System;
using System.Collections.Immutable;

namespace ImmutableAnalyzer.Utils.TypeChecking;

internal partial struct TypeChecker
{
    /// <summary>
    /// Set of valid immutable types.
    /// </summary>
    public static readonly ImmutableArray<string> ImmutableTypes = ImmutableArray.Create(
        nameof(Boolean), nameof(Byte), nameof(SByte), nameof(Char), nameof(Decimal), nameof(Double), nameof(Single),
        nameof(Int32), nameof(UInt32), nameof(Int64), nameof(UInt64), nameof(Int16), nameof(UInt16), nameof(String),
        nameof(DateTime), nameof(Guid), nameof(Enum)
    );
}