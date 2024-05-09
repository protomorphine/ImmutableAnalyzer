using System.IO;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace ImmutableAnalyzer.Tests;

public static class AnalyzerTestFactory
{
    public static CSharpAnalyzerTest<TAnalyzer, XUnitVerifier> CreateCSharpAnalyzerTest<TAnalyzer>(string source)
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        var referenceAssembly = new ReferenceAssemblies("net6.0",
            new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"),
            Path.Combine("ref", "net6.0"));

        return new CSharpAnalyzerTest<TAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources = {source},
                ReferenceAssemblies = referenceAssembly
            }
        };
    }
}