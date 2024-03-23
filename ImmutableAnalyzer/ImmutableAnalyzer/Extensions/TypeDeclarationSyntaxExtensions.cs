using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImmutableAnalyzer.Extensions;

/// <summary>
/// Extensions for <see cref="ClassDeclarationSyntax"/>
/// </summary>
public static class TypeDeclarationSyntaxExtensions
{
    private const string Attribute = nameof(System.Attribute);

    /// <summary>Cached attribute names.</summary>
    private static readonly ConcurrentDictionary<Type, string> AttributeNameCache = new();

    /// <summary>
    /// Checks if type marked by given attribute.
    /// </summary>
    /// <param name="typeDeclarationNode">Type declaration node.</param>
    /// <typeparam name="T">Attribute type.</typeparam>
    /// <returns>true - if class has given attribute, otherwise - false.</returns>
    public static bool HasAttribute<T>(this TypeDeclarationSyntax typeDeclarationNode)
        where T : Attribute
    {
        var attributeName = AttributeNameCache.GetOrAdd(
            typeof(T),
            static type => type.Name.Replace(Attribute, string.Empty)
        );

        return typeDeclarationNode
                .AttributeLists
                .SelectMany(a => a.Attributes)
                .Any(attribute => attribute.Name.ToString() == attributeName);
    }
}