﻿using System;
using System.Threading.Tasks;
using ImmutableAnalyzer.PropertyAnalyzers.SetAccessor;
using ImmutableAnalyzer.Tests.Factories;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorAnalyzer,
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorCodeFixProvider
>;
using SetAccessorCodeFixCodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
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
        var test = CreateTest(SetAccessorCodeFixProvider.FixStrategy.ToInit);
        test.CodeActionValidationMode = CodeActionValidationMode.None;

        var exception = await Record.ExceptionAsync(() => test.RunAsync());
        Assert.Null(exception);
    }

    [Fact]
    public async Task Immutable_class_with_public_set_accessor_should_be_fixed_with_private_set()
    {
        var test = CreateTest(SetAccessorCodeFixProvider.FixStrategy.ToPrivate);

        var exception = await Record.ExceptionAsync(() => test.RunAsync());
        Assert.Null(exception);
    }

    [Fact]
    public async Task Immutable_class_with_public_set_accessor_should_be_removed()
    {
        var test = CreateTest(SetAccessorCodeFixProvider.FixStrategy.Remove);

        var exception = await Record.ExceptionAsync(() => test.RunAsync());
        Assert.Null(exception);
    }

    /// <summary>
    /// Creates testcase for <see cref="SetAccessorAnalyzer"/> and <see cref="SetAccessorCodeFixProvider"/>.
    /// </summary>
    /// <param name="fixStrategy">Code fix strategy.</param>
    /// <returns>Tes case.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raises when <see cref="fixStrategy"/> is invalid.</exception>
    private static SetAccessorCodeFixCodeFixTest CreateTest(SetAccessorCodeFixProvider.FixStrategy fixStrategy)
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor("set", out var line, out var column);
        var expectedDiagnostic = Verifier.Diagnostic().WithLocation(line, column).WithArguments("set");

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

        return new SetAccessorCodeFixCodeFixTest
        {
            TestCode = source,
            FixedCode = fixedSource,
            CodeActionIndex = (int) fixStrategy,
            ExpectedDiagnostics = {expectedDiagnostic}
        };
    }
}