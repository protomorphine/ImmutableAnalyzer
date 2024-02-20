using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ImmutableAnalyzer.Analyzers;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<ImmutableAnalyzer.Analyzers.ImmutablePropertyTypeAnalyzer>;

namespace ImmutableAnalyzer.Tests;

/// <summary>
/// Tests for <see cref="ImmutableAnalyzer.Analyzers.ImmutablePropertyTypeAnalyzer"/>
/// </summary>
public class PropertyTypeAnalyzerTests
{
    [Theory]
    [MemberData(nameof(ImmutableTypes))]
    [MemberData(nameof(ImmutableGenericTypes))]
    public async Task BuildInClassPropertyType_ShouldNotAlert(string property)
    {
        var source = SourceFactory.ImmutableClassWithProperty(property);
        await Verifier.VerifyAnalyzerAsync(source).ConfigureAwait(false);
    }

    [Theory]
    [MemberData(nameof(MutableTypes))]
    public async Task BuildInClassPropertyType_ShouldAlert(string property)
    {
        var source = SourceFactory.ImmutableClassWithProperty(property);

        var expected = Verifier.Diagnostic().WithLocation(21, 12).WithArguments(property);
        await Verifier.VerifyAnalyzerAsync(source, expected).ConfigureAwait(false);
    }

    /// <summary>
    /// Immutable class declarations.
    /// </summary>
    public static IEnumerable<object[]> ImmutableTypes =>
        ImmutablePropertyTypeAnalyzer.ImmutableClassTypes.Select(it => new object[] {it});

    /// <summary>
    /// Immutable generic class declarations.
    /// </summary>
    public static IEnumerable<object[]> ImmutableGenericTypes => ImmutablePropertyTypeAnalyzer.ImmutableGenericClassTypes
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
        new object[] { nameof(DateTime) },
        new object[] { typeof(List<>).Name[..^2] + "<int>" },
        new object[] { typeof(Dictionary<,>).Name[..^2] + "<int, int>" }
    };
}