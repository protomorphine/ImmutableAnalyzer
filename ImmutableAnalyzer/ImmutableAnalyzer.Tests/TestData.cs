using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ImmutableAnalyzer.Tests;

/// <summary>
/// Represent test data.
/// </summary>
public static class TestData
{
    /// <summary>
    /// Represent immutable built-in types.
    /// <seealso cref="TypeChecker.ImmutableClassTypes"/>
    /// </summary>
    public class ImmutableTypes : TheoryData<string>
    {
        public ImmutableTypes()
        {
            foreach (var typeName in TypeChecker.ImmutableClassTypes)
                Add(typeName);
        }
    }

    /// <summary>
    /// Represent immutable built-in generic types.
    /// <seealso cref="TypeChecker.ImmutableGenericClassTypes"/>
    /// </summary>
    public class ImmutableGenericsTypes : TheoryData<string>
    {
        public ImmutableGenericsTypes()
        {
            foreach (var typeName in TypeChecker.ImmutableGenericClassTypes)
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