using System;
using System.Threading.Tasks;
using ImmutableAnalyzer.PropertyAnalyzers.SetAccessor;
using ImmutableAnalyzer.Tests.Factories;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using FixStrategy = ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorCodeFixProvider.FixStrategy;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorAnalyzer,
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorCodeFixProvider
>;
using SetAccessorCodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorAnalyzer,
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier
>;

namespace ImmutableAnalyzer.Tests.PropertyAnalyzersTests.SetAccessorTests;

public class SetAccessorCodeFixTests
{
    [Fact]
    public async Task Immutable_class_with_public_set_accessor_should_be_fixed_with_init_keyword()
    {
        var test = CreateTest(FixStrategy.ToInit);
        test.CodeActionValidationMode = CodeActionValidationMode.None;

        var exception = await Record.ExceptionAsync(() => test.RunAsync());
        Assert.Null(exception);
    }

    [Fact]
    public async Task Immutable_class_with_public_set_accessor_should_be_fixed_with_private_set()
    {
        var test = CreateTest(FixStrategy.ToPrivate);

        var exception = await Record.ExceptionAsync(() => test.RunAsync());
        Assert.Null(exception);
    }

    [Fact]
    public async Task Immutable_class_with_public_set_accessor_should_be_removed()
    {
        var test = CreateTest(FixStrategy.Remove);

        var exception = await Record.ExceptionAsync(() => test.RunAsync());
        Assert.Null(exception);
    }

    /// <summary>
    /// Creates testcase for <see cref="SetAccessorAnalyzer"/> and <see cref="SetAccessorCodeFixProvider"/>.
    /// </summary>
    /// <param name="fixStrategy">Code fix strategy.</param>
    /// <returns>Tes case.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raises when <see cref="fixStrategy"/> is invalid.</exception>
    private static SetAccessorCodeFixTest CreateTest(FixStrategy fixStrategy)
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor("set", out var line, out var column);
        var expectedDiagnostic = Verifier.Diagnostic().WithLocation(line, column).WithArguments("set");

        var fixedSource = fixStrategy switch
        {
            FixStrategy.Remove    => SourceFactory.ImmutableClassWithGetOnlyPropertyAccessor(),
            FixStrategy.ToInit    => SourceFactory.ImmutableClassWithPropertyAccessor("init", out _, out _),
            FixStrategy.ToPrivate => SourceFactory.ImmutableClassWithPropertyAccessor("private set", out _, out _),
            _                     => throw new ArgumentOutOfRangeException(nameof(fixStrategy), fixStrategy, null)
        };

        return new SetAccessorCodeFixTest
        {
            TestCode = source,
            FixedCode = fixedSource,
            CodeActionIndex = (int)fixStrategy,
            ExpectedDiagnostics = { expectedDiagnostic }
        };
    }
}
