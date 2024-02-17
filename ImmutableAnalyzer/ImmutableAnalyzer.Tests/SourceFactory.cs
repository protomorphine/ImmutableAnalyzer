namespace ImmutableAnalyzer.Tests;

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
public class ImmutableAttribute : Attribute { }
";

    /// <summary>
    /// Creates source code with immutable class with given property type.
    /// </summary>
    /// <param name="propertyDeclaration">Property type.</param>
    /// <returns>String, that represent source code.</returns>
    public static string ImmutableClassWithProperty(string propertyDeclaration) =>
        $@"
{CommonSource}

[Immutable]
public class TestImmutableClass
{{
    public {propertyDeclaration} Id {{get; init;}}
}}
";

    /// <summary>
    /// Creates source code with immutable class with given property set accessor.
    /// </summary>
    /// <param name="propertyAccessor">Property set accessor.</param>
    /// <returns>String, that represent source code.</returns>
    public static string ImmutableClassWithPropertyAccessor(string propertyAccessor) =>
        $@"
{CommonSource}

[Immutable]
public class TestImmutableClass
{{
    public int Id {{get; {propertyAccessor};}}
}}
";
}