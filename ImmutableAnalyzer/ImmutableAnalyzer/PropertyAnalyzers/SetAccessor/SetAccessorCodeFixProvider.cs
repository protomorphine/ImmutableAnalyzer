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
using AccessorModifier = System.Func<
    Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax,
    Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax
>;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor;

/// <summary>
/// Provides code fix for <see cref="SetAccessorAnalyzer"/> diagnostic.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SetAccessorCodeFixProvider)), Shared]
public class SetAccessorCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Title template to this code fix provider.
    /// </summary>
    private const string Title = "Change property accessor to '{0}'";

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(SetAccessorAnalyzer.DiagnosticId);

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root?.FindNode(diagnostic.Location.SourceSpan) is not AccessorDeclarationSyntax diagnosticNode)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: string.Format(Title, "private set"),
                createChangedDocument: ct => ChangePropertyAccessorAsync(
                    context.Document, diagnosticNode,
                    accessor => accessor.WithModifiers(
                        new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))
                    ), ct),
                equivalenceKey: $"{SetAccessorAnalyzer.DiagnosticId}Fix"),
            diagnostic
        );

        context.RegisterCodeFix(
            CodeAction.Create(
                title: string.Format(Title, "init"),
                createChangedDocument: ct => ChangePropertyAccessorAsync(
                    context.Document, diagnosticNode,
                    accessor => accessor.WithKeyword(SyntaxFactory.Token(SyntaxKind.InitKeyword)), ct),
                equivalenceKey: $"{SetAccessorAnalyzer.DiagnosticId}Fix"),
            diagnostic
        );
    }

    /// <summary>
    /// Changes access modifier with given lambda.
    /// </summary>
    /// <param name="document">Document to change.</param>
    /// <param name="accessor">Accessor to change.</param>
    /// <param name="modifier">Func to get modified accessor syntax node.</param>
    /// <param name="ct">Token to cancel task.</param>
    /// <returns></returns>
    private static async Task<Document> ChangePropertyAccessorAsync(
        Document document,
        AccessorDeclarationSyntax accessor,
        AccessorModifier modifier,
        CancellationToken ct = default
    )
    {
        var editor = await DocumentEditor.CreateAsync(document, ct);
        editor.ReplaceNode(accessor, modifier.Invoke(accessor));
        return editor.GetChangedDocument();
    }
}