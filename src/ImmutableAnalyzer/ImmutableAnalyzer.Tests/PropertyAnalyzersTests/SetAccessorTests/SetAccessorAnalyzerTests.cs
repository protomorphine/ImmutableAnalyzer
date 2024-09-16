using System.Threading.Tasks;
using ImmutableAnalyzer.PropertyAnalyzers.SetAccessor;
using ImmutableAnalyzer.Tests.Factories;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorAnalyzer>;

namespace ImmutableAnalyzer.Tests.PropertyAnalyzersTests.SetAccessorTests;

/// <summary>
/// Tests for <see cref="SetAccessorAnalyzer"/>
/// </summary>
public class SetAccessorAnalyzerTests
{
    [Fact]
    public async Task Immutable_class_property_could_not_have_a_public_setter()
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor("set", out var line, out var column);

        var expected = Verifier.Diagnostic()
            .WithLocation(line, column)
            .WithArguments("set");

        await Verifier.VerifyAnalyzerAsync(source, expected);
    }

    [Fact]
    public async Task Immutable_class_property_could_have_init_setter()
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor("init", out _, out _);
        await Verifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task Immutable_class_property_could_have_private_setter()
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor("private set", out _, out _);
        await Verifier.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public async Task Immutable_class_property_could_have_no_setter()
    {
        var source = SourceFactory.ImmutableClassWithGetOnlyPropertyAccessor();
        await Verifier.VerifyAnalyzerAsync(source);
    }
}
