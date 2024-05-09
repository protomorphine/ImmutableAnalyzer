using System.Threading.Tasks;
using ImmutableAnalyzer.ParameterAnalyzers;
using ImmutableAnalyzer.Tests.Factories;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
        ImmutableAnalyzer.ParameterAnalyzers.ParameterTypeAnalyzer>;

namespace ImmutableAnalyzer.Tests.RecordAnalyzersTests;

public class ParameterTypeAnalyzerTests
{
    [Theory]
    [ClassData(typeof(TestData.ImmutableTypes))]
    [ClassData(typeof(TestData.ImmutableGenericsTypes))]
    public async Task Record_with_immutable_property_should_be_immutable(string property)
    {
        var source = SourceFactory.ImmutableRecordWithParameter(property, out _, out _);
        var test = AnalyzerTestFactory.CreateCSharpAnalyzerTest<ParameterTypeAnalyzer>(source);

        var exception = await Record.ExceptionAsync(() => test.RunAsync());
        Assert.Null(exception);
    }

    [Theory]
    [ClassData(typeof(TestData.MutableTypes))]
    public async Task Record_with_mutable_property_should_not_be_immutable(string property)
    {
        var source = SourceFactory.ImmutableRecordWithParameter(property, out var line, out var column);

        var expected = Verifier.Diagnostic().WithLocation(line, column).WithArguments(property);
        await Verifier.VerifyAnalyzerAsync(source, expected);
    }
}