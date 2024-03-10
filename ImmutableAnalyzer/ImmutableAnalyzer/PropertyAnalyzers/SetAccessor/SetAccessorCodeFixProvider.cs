using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SetAccessorCodeFixProvider))]
[Shared]
public class SetAccessorCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SetAccessorAnalyzer.DiagnosticId);
    public override FixAllProvider? GetFixAllProvider() => null;
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnosticNode = root?.FindNode(diagnosticSpan);

        if (diagnosticNode is not PropertyDeclarationSyntax declaration)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Change property accessor to 'private set'",
                createChangedDocument: ct => ChangePropertyAccessorAsync(
                    context.Document,
                    declaration,
                    SyntaxKind.PrivateKeyword,
                    ct),
                equivalenceKey: $"{SetAccessorAnalyzer.DiagnosticId}Fix"),
            diagnostic
        );

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Change property accessor to 'init'",
                createChangedDocument: ct => ChangePropertyAccessorAsync(
                    context.Document,
                    declaration,
                    SyntaxKind.InitKeyword,
                    ct),
                equivalenceKey: $"{SetAccessorAnalyzer.DiagnosticId}Fix"),
            diagnostic
        );
    }

    private static async Task<Document> ChangePropertyAccessorAsync(
        Document document,
        BasePropertyDeclarationSyntax property,
        SyntaxKind keyword,
        CancellationToken ct = default
    )
    {
        var editor = await DocumentEditor.CreateAsync(document, ct);

        var setter = property.AccessorList?.Accessors
            .FirstOrDefault(a => a.IsKind(SyntaxKind.SetAccessorDeclaration));
        if (setter == null)
            return document;

        var newSetter = setter.WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(keyword)));

        editor.ReplaceNode(setter, newSetter);
        return editor.GetChangedDocument();
    }
}