using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ImmutableAnalyzer.Analyzers;

/// <summary>
/// Analyzer to check set accessor of properties of immutable classes.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class ImmutableSetAccessorAnalyzer : BaseImmutableAnalyzer
{
    protected override string DiagnosticId => "IM0002";
    protected override string Title => "Public setter violates class immutability";
    protected override string MessageFormat => "Member of immutable class can't have '{0}' accessor";
    protected override string Description => "Setter can't be public, because it's give possibility to change member from the outer.";

    protected override string Category => "Design";

    protected override void AnalyzeSyntax(ClassDeclarationSyntax classDeclarationNode, SyntaxNodeAnalysisContext context)
    {
        foreach (var member in classDeclarationNode.Members)
        {
            if (member is not PropertyDeclarationSyntax prop)
                return;

            var setAccessor = prop.AccessorList?.Accessors.FirstOrDefault(it => it.Keyword.ValueText is not "get");
            if (setAccessor is null || !ShouldReport(setAccessor))
                continue;

            var diagnostic = Diagnostic.Create(Rule, setAccessor.GetLocation(),
                setAccessor.Modifiers.ToFullString() + setAccessor.Keyword.ValueText
            );
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static bool ShouldReport(AccessorDeclarationSyntax setAccessor)
    {
        var accessorModifiers = setAccessor.Modifiers.Select(it => it.ValueText).ToList();
        var accessorKeyword = setAccessor.Keyword;

        switch (accessorKeyword.ValueText)
        {
            case "init":
            case "set" when accessorModifiers.Contains("private"):
                return false;
            default:
                return true;
        }
    }
}