using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ImmutableAnalyzer.Extensions;

/// <summary>
/// Extensions for <see cref="ClassDeclarationSyntax"/>
/// </summary>
public static class ClassDeclarationSyntaxExtensions
{
    /// <summary>
    /// Checks if class marked by given attribute.
    /// </summary>
    /// <param name="classDeclarationNode">Class declaration node.</param>
    /// <typeparam name="T">Attribute type.</typeparam>
    /// <returns>true - if class has given attribute, otherwise - false.</returns>
    public static bool HasAttribute<T>(this ClassDeclarationSyntax classDeclarationNode)
        where T : Attribute =>
        classDeclarationNode.AttributeLists
            .Select(al => al.Attributes)
            .Any(asl =>
                asl.Any(i =>
                    i.Name.ToString() == typeof(T).Name.Replace("Attribute", "")));
}