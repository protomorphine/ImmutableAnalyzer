using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace ImmutableAnalyzer.Services.Rules;

/// <summary>
/// Checks <see cref="ITypeSymbol"/> for immutability by declared types.
/// </summary>
internal class KnownTypesRule : IImmutabilityCheckRule
{
    private static readonly SymbolDisplayFormat SymbolDisplayFormat =
        new(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.None
        );

    private readonly ImmutableHashSet<string> _immutableTypes;

    public KnownTypesRule()
    {
        _immutableTypes = GetTypeNamesFromEmbeddedResources();
    }

    /// <inheritdoc />
    public bool IsImmutable(ITypeSymbol typeSymbol) => _immutableTypes.Contains(typeSymbol.OriginalDefinition.ToDisplayString(SymbolDisplayFormat));

    private ImmutableHashSet<string> GetTypeNamesFromEmbeddedResources()
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream("ImmutableTypes.txt");
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd().Split(['\n'], StringSplitOptions.RemoveEmptyEntries).ToImmutableHashSet();
    }
}
