using Xunit;
using System.Threading.Tasks;

using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    ImmutableAnalyzer.Analyzers.ImmutableSetAccessorAnalyzer>;

namespace ImmutableAnalyzer.Tests;

/// <summary>
/// Tests for <see cref="ImmutableAnalyzer.Analyzers.ImmutableSetAccessorAnalyzer"/>
/// </summary>
public class SetAccessorAnalyzerTests
{
    [Theory]
    [InlineData("set")]
    public async Task ImmutableClassPropertySetAccessor_ShouldReport(string propertyAccessor)
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor(propertyAccessor);

        var expected = Verifier.Diagnostic()
            .WithLocation(21, 25)
            .WithArguments(propertyAccessor);

        await Verifier.VerifyAnalyzerAsync(source, expected).ConfigureAwait(false);
    }

    [Theory]
    [InlineData("init")]
    [InlineData("private set")]
    public async Task ImmutableClassPropertySetAccessor_ShouldNotReport(string propertyAccessor)
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor(propertyAccessor);

        await Verifier.VerifyAnalyzerAsync(source).ConfigureAwait(false);
    }
}