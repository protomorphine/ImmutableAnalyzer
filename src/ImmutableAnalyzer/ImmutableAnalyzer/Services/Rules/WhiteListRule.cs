using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Services.Rules;

/// <summary>
/// Rule to check immutability of types by whitelist file.
/// </summary>
internal class WhiteListRule : IImmutabilityCheckRule
{
    private const string WhiteListFile = "ImmutableTypes.txt";
    private readonly ImmutableHashSet<string> _whiteListTypes;

    /// <summary>
    /// Creates new instance of <see cref="WhiteListRule"/>.
    /// </summary>
    /// <param name="opts">Immutable analyzer options.</param>
    public WhiteListRule(AnalyzerOptions opts)
    {
        _whiteListTypes = GetWhiteListedTypes(opts);
    }

    /// <inheritdoc/>
    public bool IsImmutable(ITypeSymbol typeSymbol) => _whiteListTypes.Contains(typeSymbol.MetadataName);

    /// <summary>
    /// Gets content of <see cref="WhiteListFile"/> as <see cref="ImmutableHashSet{string}"/>.
    /// </summary>
    /// <param name="opts">Immutable analyzer options.</param>
    /// <returns>Set of white listed types.</returns>
    /// <exception cref="InvalidOperationException">Throws when required file doesn't exist or there was some errors while reading it.</exception>
    private static ImmutableHashSet<string> GetWhiteListedTypes(AnalyzerOptions opts)
    {
        var file = opts
            .AdditionalFiles
            .SingleOrDefault(file => Path.GetFileName(file.Path).Equals(WhiteListFile));

        if (file is null)
            return ImmutableHashSet<string>.Empty;

        var text = file.GetText()
            ?? throw new InvalidOperationException($"There were some errors while reading '{WhiteListFile}' file");

        return text.Lines.Select(line => line.ToString()).ToImmutableHashSet();
    }
}
