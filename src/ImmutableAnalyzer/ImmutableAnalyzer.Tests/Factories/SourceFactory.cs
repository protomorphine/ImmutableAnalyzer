using System;
using System.Linq;

namespace ImmutableAnalyzer.Tests.Factories;

/// <summary>
/// Simple factory to create source code to test.
/// </summary>
public static class SourceFactory
{
    private const string CommonSource = @"
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace System.Runtime.CompilerServices
{
[EditorBrowsable(EditorBrowsableState.Never)]
public record IsExternalInit;
}

[AttributeUsage(AttributeTargets.Class)]
public class ImmutableAttribute : Attribute { }";

    /// <summary>
    /// Creates source code with immutable class with given property type.
    /// </summary>
    /// <param name="propertyType">Property type.</param>
    /// <param name="line">Out line of `propertyType`.</param>
    /// <param name="column">Out column of `propertyType`.</param>
    /// <returns>String, that represent source code.</returns>
    public static string ImmutableClassWithProperty(string propertyType, out int line, out int column)
    {
        var source = $@"{CommonSource}

[Immutable]
public class TestImmutableClass
{{
    public {propertyType} Id {{ get; init; }}
}}";

        (line, column) = GetLineAndColumn(source, propertyType);
        return source;
    }

    /// <summary>
    /// Creates source code with immutable class with given property set accessor.
    /// </summary>
    /// <param name="propertySetAccessor">Property set accessor.</param>
    /// <param name="line">Out line of `propertySetAccessor`.</param>
    /// <param name="column">Out column of `propertySetAccessor`.</param>
    /// <returns>String, that represent source code.</returns>
    public static string ImmutableClassWithPropertyAccessor(string propertySetAccessor, out int line, out int column)
    {
        var source = $@"{CommonSource}

[Immutable]
public class TestImmutableClass
{{
    public int Id {{ get; {propertySetAccessor}; }}
}}";

        (line, column) = GetLineAndColumn(source, propertySetAccessor);
        return source;
    }

    /// <summary>
    /// Creates source code with immutable class with getter only.
    /// </summary>
    /// <returns>String, that represent source code.</returns>
    public static string ImmutableClassWithGetOnlyPropertyAccessor()
    {
        const string source = $@"{CommonSource}

[Immutable]
public class TestImmutableClass
{{
    public int Id {{ get; }}
}}";

        return source;
    }

    /// <summary>
    /// Creates source code with immutable record with given primary ctor parameter type.
    /// </summary>
    /// <param name="parameterType">Parameter type.</param>
    /// <param name="line">Out line of `parameterType`.</param>
    /// <param name="column">Out column of `parameterType`.</param>
    /// <returns>String, that represent source code.</returns>
    public static string ImmutableRecordWithParameter(string parameterType, out int line, out int column)
    {
        var source = $@"{CommonSource}

[Immutable]
public record TestImmutableRecord({parameterType} Param);";

        (line, column) = GetLineAndColumn(source, parameterType);
        return source;
    }

    /// <summary>
    /// Get coordinates of given substring in source text.
    /// </summary>
    /// <param name="source">Source text.</param>
    /// <param name="textToFind">Substring to find coordinates.</param>
    /// <returns>Tuple with line number and column.</returns>
    private static (int Line, int Column) GetLineAndColumn(string source, string textToFind)
    {
        var lines = source.Split(Environment.NewLine).ToList();
        var line = lines.FindIndex(x => x.Contains(textToFind));
        var column = lines[line].IndexOf(textToFind, StringComparison.InvariantCulture);

        return (line + 1, column + 1);
    }
}