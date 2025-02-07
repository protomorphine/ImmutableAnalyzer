using System;
using ImmutableAnalyzer.Services.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Services;

/// <summary>
/// Service for check types immutability.
/// </summary>
internal class ImmutableTypeCheckerService
{
    /// <summary>
    /// Factory method to create <see cref="ImmutableTypeCheckerService"/>.
    /// </summary>
    /// <param name="context">Context.</param>
    /// <returns>Configured instance of <see cref="ImmutableTypeCheckerService"/>.</returns>
    public static ImmutableTypeCheckerService Create(SyntaxNodeAnalysisContext context)
    {
        var rules = new IImmutabilityCheckRule[]
        {
            new EnumRule(),
            new KnownTypesRule(),
            new WhiteListRule(context.Options),
            new AttributeRule(context),
        };

        return new ImmutableTypeCheckerService(rules);
    }

    private readonly IImmutabilityCheckRule[] _rules;

    /// <summary>
    /// Creates new instance of <see cref="ImmutableTypeCheckerService"/>.
    /// </summary>
    /// <param name="rules">Set of rules to check type immutability.</param>
    private ImmutableTypeCheckerService(IImmutabilityCheckRule[] rules)
    {
        _rules = rules;
    }

    /// <summary>
    /// Checks <paramref name="typeSymbol"/> for immutability by rules.
    /// <seealso cref="IImmutabilityCheckRule"/>
    /// </summary>
    /// <param name="typeSymbol">Type symbol to check.</param>
    /// <returns>true - if given <paramref name="typeSymbol"/> is immutable, otherwise - false.</returns>
    public bool IsImmutable(ITypeSymbol typeSymbol) =>
        Array.Exists(_rules, r => r.IsImmutable(typeSymbol));
}
