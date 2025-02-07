using System.IO;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace ImmutableAnalyzer.Tests.Factories;

/// <summary>
/// Simple factory for <see cref="CSharpAnalyzerTest{TAnalyzer,TVerifier}"/>.
/// </summary>
public static class AnalyzerTestFactory
{
    /// <summary>
    /// Creates test case with reference to .NET6 for given analyzer.
    /// </summary>
    /// <param name="source">Source code to test.</param>
    /// <typeparam name="TAnalyzer">Type of analyzer.</typeparam>
    /// <returns>Test case.</returns>
    public static CSharpAnalyzerTest<TAnalyzer, DefaultVerifier> CreateCSharpAnalyzerTest<TAnalyzer>(string source)
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        var referenceAssembly = new ReferenceAssemblies("net6.0",
            new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"),
            Path.Combine("ref", "net6.0"));

        return new CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
        {
            TestState =
            {
                Sources = {source},
                ReferenceAssemblies = referenceAssembly,
                AdditionalFiles = {("ImmutableTypes.txt", string.Empty)}
            }
        };

        test.TestState.AdditionalFiles.Add(("ImmutableTypes.txt", string.Empty));
        return test;
    }
}
