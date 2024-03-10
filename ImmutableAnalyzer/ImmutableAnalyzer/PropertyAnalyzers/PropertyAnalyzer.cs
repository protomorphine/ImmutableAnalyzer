using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImmutableAnalyzer.PropertyAnalyzers;

/// <summary>
/// Base class for property analyzer in immutable type.
/// </summary>
internal abstract class PropertyAnalyzer : ClassMemberAnalyzer<PropertyDeclarationSyntax> { }