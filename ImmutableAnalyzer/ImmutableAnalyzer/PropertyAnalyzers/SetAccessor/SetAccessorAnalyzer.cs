using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor;

/// <summary>
/// Analyzer to check set accessor of properties of immutable classes.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SetAccessorAnalyzer : PropertyAnalyzer
{
    /// <summary>
    /// Analyzer Diagnostic Id.
    /// </summary>
    public const string DiagnosticId = "IM0002";

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rule);

    /// <summary>
    /// Diagnostic descriptor.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        id:                 DiagnosticId,
        title:              "Public setter violates type immutability",
        messageFormat:      "Member of immutable type can't have '{0}' accessor",
        category:           "Design",
        defaultSeverity:    DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description:        "Setter can't be public, because it's give possibility to change member from the outer."
    );

    /// <inheritdoc />
    protected override void AnalyzeMember(PropertyDeclarationSyntax node, SyntaxNodeAnalysisContext ctx)
    {
        var setAccessor = node.AccessorList?.Accessors.FirstOrDefault(it =>
            it.IsKind(SyntaxKind.SetAccessorDeclaration)
        );

        if (setAccessor is null || !ShouldReport(setAccessor))
            return;

        var diagnostic = Diagnostic.Create(Rule, setAccessor.GetLocation(),
            setAccessor.Modifiers.ToFullString() + setAccessor.Keyword.ValueText
        );
        ctx.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determine if `IM0002` diagnostic should report.
    /// </summary>
    /// <param name="setAccessor">Accessor declaration syntax.</param>
    /// <returns>false - if accessor is `init` or `private set`, otherwise - true.</returns>
    private static bool ShouldReport(AccessorDeclarationSyntax setAccessor)
    {
        var modifiers = setAccessor.Modifiers;
        return !(modifiers.Any(SyntaxKind.PrivateKeyword) || modifiers.Any(SyntaxKind.InitKeyword));
    }
}