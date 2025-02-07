using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace ImmutableAnalyzer.Tests;

/// <summary>
/// Represent test data.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TestData
{
    /// <summary>
    /// Set of generic immutable class types.
    /// </summary>
    public static readonly ImmutableArray<string> Generics = ImmutableArray.Create(
        typeof(ImmutableArray<>).Name, typeof(ImmutableDictionary<,>).Name, typeof(ImmutableList<>).Name,
        typeof(ImmutableHashSet<>).Name, typeof(ImmutableSortedDictionary<,>).Name, typeof(ImmutableSortedSet<>).Name,
        typeof(ImmutableStack<>).Name, typeof(ImmutableQueue<>).Name, typeof(IReadOnlyList<>).Name,
        typeof(IReadOnlyCollection<>).Name, typeof(IReadOnlyDictionary<,>).Name
    );

    /// <summary>
    /// Set of valid immutable types.
    /// </summary>
    public static readonly ImmutableArray<string> BasicTypes = ImmutableArray.Create(
        nameof(Boolean), nameof(Byte), nameof(SByte), nameof(Char), nameof(Decimal), nameof(Double), nameof(Single),
        nameof(Int32), nameof(UInt32), nameof(Int64), nameof(UInt64), nameof(Int16), nameof(UInt16), nameof(String),
        nameof(DateTime), nameof(Guid), nameof(Enum)
    );

    /// <summary>
    /// Represent immutable built-in types.
    /// <seealso cref="Types.ImmutableTypes"/>
    /// </summary>
    public class ImmutableTypes : TheoryData<string>
    {
        public ImmutableTypes()
        {
            foreach (var typeName in BasicTypes)
                Add(typeName);
        }
    }

    /// <summary>
    /// Represent immutable built-in generic types.
    /// <seealso cref="Types.Generic"/>
    /// </summary>
    public class ImmutableGenericsTypes : TheoryData<string>
    {
        public ImmutableGenericsTypes()
        {
            foreach (var typeName in Generics)
                Add(CreateGenericTypeStringWithParams(typeName, nameof(Int32)));
        }

        /// <summary>
        /// Create generic type string e.g. 'Dictionary&lt;int,int&gt;'
        /// </summary>
        /// <param name="genericTypeName">Type name, e.g. Dictionary`2.</param>
        /// <param name="paramType">Generic parameter type.</param>
        /// <returns>String of generic type with parameters.</returns>
        private static string CreateGenericTypeStringWithParams(string genericTypeName, string paramType)
        {
            var genericParamsCount = genericTypeName[^1] - '0';
            var genericParams = $"<{string.Join(',', Enumerable.Range(0, genericParamsCount).Select(_ => paramType))}>";

            return genericTypeName[..^2] + genericParams;
        }
    }

    /// <summary>
    /// Represent built-in mutable types.
    /// </summary>
    public class MutableTypes : TheoryData<string>
    {
        public MutableTypes()
        {
            Add(nameof(Object));

            Add(typeof(IEnumerable<>).Name[..^2] + "<int>");
            Add(typeof(ICollection<>).Name[..^2] + "<int>");

            Add(typeof(List<>).Name[..^2] + "<int>");
            Add(typeof(IList<>).Name[..^2] + "<int>");

            Add(typeof(Dictionary<,>).Name[..^2] + "<int, int>");
            Add(typeof(IDictionary<,>).Name[..^2] + "<int, int>");

            Add(typeof(HashSet<>).Name[..^2] + "<int>");
            Add(typeof(ISet<>).Name[..^2] + "<int>");

            Add(typeof(Stack<>).Name[..^2] + "<int>");

            Add(typeof(Queue<>).Name[..^2] + "<int>");
        }
    }
}
