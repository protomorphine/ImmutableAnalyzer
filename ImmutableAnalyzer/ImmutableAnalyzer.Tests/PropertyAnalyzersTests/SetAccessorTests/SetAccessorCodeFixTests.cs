using System.Threading.Tasks;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorAnalyzer,
    ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.SetAccessorCodeFixProvider
>;

namespace ImmutableAnalyzer.Tests.PropertyAnalyzersTests.SetAccessorTests;

public class SetAccessorCodeFixTests
{
    [Fact]
    public async Task Immutable_class_with_public_set_accessor_should_be_fixed_with_private_set()
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor("set", out var line, out var column);
        var fixedSource = SourceFactory.ImmutableClassWithPropertyAccessor("private set", out _, out _);

        var expected = Verifier.Diagnostic()
            .WithLocation(line, column)
            .WithArguments("set");
        await Verifier.VerifyCodeFixAsync(source, expected, fixedSource).ConfigureAwait(false);
    }
}