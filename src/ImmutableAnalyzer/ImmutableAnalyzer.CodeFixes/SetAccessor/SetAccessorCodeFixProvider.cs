using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using ImmutableAnalyzer.CodeFixes.SetAccessor.Strategies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using IM0002_Const = ImmutableAnalyzer.Const.AnalyzerConstants.SetAccessorAnalyzer;

namespace ImmutableAnalyzer.CodeFixes.SetAccessor;

/// <summary>
/// Provides code fix for <see cref="SetAccessorAnalyzer"/> diagnostic.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SetAccessorCodeFixProvider)), Shared]
public class SetAccessorCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Controls the sequence of code fix actions registrations.
    /// </summary>
    public enum FixStrategy
    {
        /// <summary>Mapped to <see cref="CodeFixes.ToInit"/>.</summary>
        ToInit = 0,
        /// <summary>Mapped to <see cref="CodeFixes.ToPrivate"/>.</summary>
        ToPrivate = 1,
        /// <summary>Mapped to <see cref="RemoveSetAccessor"/>.</summary>
        Remove = 2,
    }

    /// <summary>
    /// Title template to this code fix provider.
    /// </summary>
    private const string TitleFormat = "Change property set accessor to '{0}'";

    /// <summary>
    /// Key, used to determine the equivalence of the <see cref="CodeAction"/> with other <see cref="CodeAction"/>s.
    /// </summary>
    private const string EquivalencyKey = $"{IM0002_Const.DiagnosticId}Fix";

    /// <summary>
    /// Key-Value pairs of strategies to codefix.
    /// </summary>
    private static readonly IReadOnlyDictionary<FixStrategy, ChangeSetAccessorCodeFix> ChangeSetAccessorFixes =
        new SortedDictionary<FixStrategy, ChangeSetAccessorCodeFix>
        {
            { FixStrategy.ToInit,    new ToInit()            },
            { FixStrategy.ToPrivate, new ToPrivate()         },
            { FixStrategy.Remove,    new RemoveSetAccessor() },
        };

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(IM0002_Const.DiagnosticId);

    /// <inheritdoc />
    public override FixAllProvider? GetFixAllProvider() => null;

    /// <inheritdoc />
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.Single();

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root?.FindNode(diagnostic.Location.SourceSpan) is not AccessorDeclarationSyntax node)
            return;

        foreach (var strategy in ChangeSetAccessorFixes.Values)
        {
            var action = CodeAction.Create(
                title: strategy.GetTitle(TitleFormat),
                createChangedDocument: ct => strategy.ChangeDocument(context.Document, node, ct),
                equivalenceKey: EquivalencyKey
            );

            context.RegisterCodeFix(action, diagnostic);
        }
    }
}
