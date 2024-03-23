using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImmutableAnalyzer.PropertyAnalyzers.PropertyType;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<ImmutableAnalyzer.PropertyAnalyzers.PropertyType.PropertyTypeAnalyzer>;

namespace ImmutableAnalyzer.Tests.TypePropertiesTests.PropertyTypeTests;

/// <summary>
/// Tests for <see cref="PropertyTypeAnalyzer"/>
/// </summary>
public class PropertyTypeAnalyzerTests
{
    [Theory]
    [MemberData(nameof(ImmutableTypes))]
    [MemberData(nameof(ImmutableGenericTypes))]
    public async Task BuiltInClassPropertyType_ShouldNotAlert(string property)
    {
        var analyzerTest =  new CSharpAnalyzerTest<PropertyTypeAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources = { SourceFactory.ImmutableClassWithProperty(property) },

                ReferenceAssemblies = new ReferenceAssemblies(
                    "net6.0",
                    new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"),
                    Path.Combine("ref", "net6.0"))
            }
        };

        await analyzerTest.RunAsync().ConfigureAwait(false);

        Assert.True(true); // SonarLint S2699
    }

    [Theory]
    [MemberData(nameof(MutableTypes))]
    public async Task BuiltInClassPropertyType_ShouldAlert(string property)
    {
        var source = SourceFactory.ImmutableClassWithProperty(property);

        var expected = Verifier.Diagnostic().WithLocation(21, 12).WithArguments(property);
        await Verifier.VerifyAnalyzerAsync(source, expected).ConfigureAwait(false);
    }

    [Theory]
    [InlineData("[Immutable]")]
    [InlineData("")]
    public async Task UserDefinedTypes_ClassesWithImmutableAttribute_ShouldBeConsideredImmutable(string attribute)
    {
        var source = @$"
{SourceFactory.CommonSource}

{attribute}
public class Person
{{
    public int Id {{ get; init; }}
}}

[Immutable]
public class Pet
{{
    public string Name {{ get; }}
    public Person Owner {{ get; init; }}
}}
";
        await (!string.IsNullOrWhiteSpace(attribute)
            ? Verifier.VerifyAnalyzerAsync(source)
            : Verifier.VerifyAnalyzerAsync(source,
                Verifier.Diagnostic().WithLocation(28, 12).WithArguments("Person")
            ));
    }

    [Fact]
    public async Task UserDefinedTypes_Enums_ShouldBeConsideredImmutable()
    {
        const string source = @$"
{SourceFactory.CommonSource}

public enum PetType
{{
    Dog,
    Cat,
    Parrot
}}

[Immutable]
public class Pet
{{
    public string Name {{ get; }}
    public PetType PetType {{ get; init; }}
}}
";
        await Verifier.VerifyAnalyzerAsync(source);
    }

    #region Test Data

    /// <summary>
    /// Immutable class declarations.
    /// </summary>
    public static IEnumerable<object[]> ImmutableTypes =>
        PropertyTypeAnalyzer.ImmutableClassTypes.Select(it => new object[] {it});

    /// <summary>
    /// Immutable generic class declarations.
    /// </summary>
    public static IEnumerable<object[]> ImmutableGenericTypes => PropertyTypeAnalyzer.ImmutableGenericClassTypes
        .Select(it =>
        {
            var genericParamsCount = it[^1] - '0';
            var genericParams = $"<{string.Join(',', Enumerable.Range(0, genericParamsCount).Select(_ => "int"))}>";
            return new object[] {it[..^2] + genericParams};
        });

    /// <summary>
    /// Mutable types declarations.
    /// </summary>
    public static IEnumerable<object[]> MutableTypes => new List<object[]>()
    {
        new object[] { nameof(Object) },
        new object[] { typeof(List<>).Name[..^2] + "<int>" },
        new object[] { typeof(Dictionary<,>).Name[..^2] + "<int, int>" }
    };

    #endregion
}