using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using ImmutableAnalyzer.Analyzers;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
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
        var analyzerTest =  new CSharpAnalyzerTest<ImmutablePropertyTypeAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources = { SourceFactory.ImmutableClassWithProperty(property) },
                
                ReferenceAssemblies = new ReferenceAssemblies(
                    "net5.0", 
                    new PackageIdentity("Microsoft.NETCore.App.Ref", "5.0.0"), 
                    Path.Combine("ref", "net5.0"))
            }
        };

        await analyzerTest.RunAsync().ConfigureAwait(false);
        
        Assert.True(true);
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