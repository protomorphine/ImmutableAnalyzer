using System.Threading.Tasks;
using ImmutableAnalyzer.PropertyAnalyzers.PropertyType;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
        ImmutableAnalyzer.PropertyAnalyzers.PropertyType.PropertyTypeAnalyzer>;

namespace ImmutableAnalyzer.Tests.PropertyAnalyzersTests.PropertyTypeTests;

/// <summary>
/// Tests for <see cref="PropertyTypeAnalyzer"/>
/// </summary>
public class PropertyTypeAnalyzerTests
{
    [Theory]
    [ClassData(typeof(TestData.ImmutableTypes))]
    [ClassData(typeof(TestData.ImmutableGenericsTypes))]
    public async Task Class_with_immutable_property_should_be_immutable(string property)
    {
        var source = SourceFactory.ImmutableClassWithProperty(property, out _, out _);
        var test = AnalyzerTestFactory.CreateCSharpAnalyzerTest<PropertyTypeAnalyzer>(source);

        await test.RunAsync().ConfigureAwait(false);
        Assert.True(true); // SonarLint S2699
    }

    [Theory]
    [ClassData(typeof(TestData.MutableTypes))]
    public async Task Class_with_mutable_property_should_not_be_immutable(string property)
    {
        var source = SourceFactory.ImmutableClassWithProperty(property, out var line, out var column);

        var expected = Verifier.Diagnostic().WithLocation(line, column).WithArguments(property);
        await Verifier.VerifyAnalyzerAsync(source, expected).ConfigureAwait(false);
    }

    [Fact]
    public async Task Class_with_user_defined_immutable_property_type_should_be_immutable()
    {
        const string className = "Person";

        var source =
            SourceFactory.ImmutableClassWithProperty(propertyType: className, out _, out _) +
            PureClassWithImmutableProperty(name: className, "[Immutable]");

        await Verifier.VerifyAnalyzerAsync(source).ConfigureAwait(false);
    }

    [Fact]
    public async Task Class_with_user_defined_mutable_property_type_should_not_be_immutable()
    {
        const string className = "Person";

        var source =
            SourceFactory.ImmutableClassWithProperty(propertyType: className, out var line, out var column) +
            PureClassWithImmutableProperty(name: className);

        var expectedDiagnostic = Verifier.Diagnostic().WithLocation(line, column).WithArguments(className);
        await Verifier.VerifyAnalyzerAsync(source, expectedDiagnostic).ConfigureAwait(false);
    }

    [Fact]
    public async Task Class_with_enum_property_type_should_be_immutable()
    {
        const string enumName = "TestEnum";
        var source =
            SourceFactory.ImmutableClassWithProperty(enumName, out _, out _) +
            $"public enum {enumName} {{ Value1, Value2, Value3 }}";

        await Verifier.VerifyAnalyzerAsync(source);
    }

    /// <summary>
    /// Creates source code of class (which contains 1 property named `Id` with type `long`) with given name and attributes.
    /// </summary>
    /// <param name="name">Class name.</param>
    /// <param name="attribute">Attributes.</param>
    /// <returns>Source code of class.</returns>
    private static string PureClassWithImmutableProperty(string name, string attribute = "") =>
        $@"
            {attribute}
            public class {name}
            {{
                public long Id {{ get; init; }}
            }}
        ";
}