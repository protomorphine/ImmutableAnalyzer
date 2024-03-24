using System;

namespace ImmutableAnalyzer;

/// <summary>
/// Attribute, that represent immutable class.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public class ImmutableAttribute : Attribute { }