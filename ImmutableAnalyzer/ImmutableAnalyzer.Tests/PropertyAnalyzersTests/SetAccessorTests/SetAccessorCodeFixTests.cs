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
    public async Task ImmutableClassPropertyWithPublicSetAccessor_ReplaceWithPrivateSet()
    {
        var source = SourceFactory.ImmutableClassWithPropertyAccessor("set");
        var fixedSource = SourceFactory.ImmutableClassWithPropertyAccessor("private set");

        var expected = Verifier.Diagnostic()
            .WithLocation(21, 25)
            .WithArguments("set");
        await Verifier.VerifyCodeFixAsync(source, expected, fixedSource).ConfigureAwait(false);
    }
}