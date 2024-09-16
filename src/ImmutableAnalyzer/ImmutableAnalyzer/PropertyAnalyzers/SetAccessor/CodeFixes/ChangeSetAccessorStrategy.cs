using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace ImmutableAnalyzer.PropertyAnalyzers.SetAccessor.CodeFixes;

/// <summary>
/// Base class for change set accessor strategy.
/// </summary>
internal abstract class ChangeSetAccessorCodeFix
{
    /// <summary>
    /// Delegate, which used to get new <see cref="AccessorDeclarationSyntax"/> from given.
    /// </summary>
    /// <remarks>We must return new accessor instead of modifying given because it's immutable.</remarks>
    protected delegate void AccessorModifier(AccessorDeclarationSyntax originalNode, DocumentEditor editor);

    /// <summary>
    /// Gets title for code fix.
    /// </summary>
    /// <param name="format">Format of title.</param>
    /// <returns>string, that represent code fix title.</returns>
    public abstract string GetTitle(string format);

    /// <summary>
    /// Concrete <see cref="AccessorModifier"/>, which used to modify <see cref="AccessorDeclarationSyntax"/>.
    /// </summary>
    protected abstract AccessorModifier Modifier { get; }

    /// <summary>
    /// Applies <see cref="Modifier"/> to given <paramref name="node"/>.
    /// </summary>
    /// <param name="originalDocument">Original <see cref="Document"/>.</param>
    /// <param name="node">Node to apply changes.</param>
    /// <param name="ct">Token for cancel task.</param>
    /// <returns>Changed <see cref="Document"/>.</returns>
    public async Task<Document> ChangeDocument(Document originalDocument, AccessorDeclarationSyntax node, CancellationToken ct)
    {
        var editor = await DocumentEditor.CreateAsync(originalDocument, ct).ConfigureAwait(false);
        Modifier.Invoke(node, editor);

        return editor.GetChangedDocument();
    }
}
