using System;

namespace ImmutableAnalyzer;

/// <summary>
/// Attribute, that represent immutable class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ImmutableAttribute : Attribute { }