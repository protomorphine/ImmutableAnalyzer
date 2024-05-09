using System;
using System.Threading.Tasks;
using ImmutableAnalyzer.PropertyAnalyzers.SetAccessor;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorAnalyzer,
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorCodeFixProvider
>;
using CodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorAnalyzer,
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier
>;

namespace ImmutableAnalyzer.Tests.PropertyAnalyzersTests.SetAccessorTests;

public class SetAccessorCodeFixTests
{
    [Fact]
    public async Task Immutable_class_with_public_set_accessor_should_be_fixed_with_private_set()
    {
        await CreateTest(SetAccessorCodeFixProvider.FixStrategy.ToPrivate).RunAsync();
    }
    
    private static CodeFixTest CreateTest(SetAccessorCodeFixProvider.FixStrategy fixStrategy)
    {
        const string set = "set";
        var source = SourceFactory.ImmutableClassWithPropertyAccessor(set, out var line, out var column);
        var expectedDiagnostic = Verifier.Diagnostic().WithLocation(line, column).WithArguments(set);

        var fixedSource = fixStrategy switch
        {
            SetAccessorCodeFixProvider.FixStrategy.Remove =>
                SourceFactory.ImmutableClassWithGetOnlyPropertyAccessor(),

            SetAccessorCodeFixProvider.FixStrategy.ToInit =>
                SourceFactory.ImmutableClassWithPropertyAccessor("init", out _, out _),

            SetAccessorCodeFixProvider.FixStrategy.ToPrivate =>
                SourceFactory.ImmutableClassWithPropertyAccessor("private set", out _, out _),

            _ => throw new ArgumentOutOfRangeException(nameof(fixStrategy), fixStrategy, null)
        };

        return new CodeFixTest
        {
            TestCode = source,
            FixedCode = fixedSource,
            CodeActionIndex = (int) fixStrategy,
            ExpectedDiagnostics = {expectedDiagnostic}
        };
    }
}