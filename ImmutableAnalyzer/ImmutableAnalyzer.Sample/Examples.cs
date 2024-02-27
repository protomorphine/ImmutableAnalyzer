// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace ImmutableAnalyzer.Sample;

// If you don't see warnings, build the Analyzers Project.

[Immutable]
public class Person
{
    public long Id { get; set; }

    public List<string> Names { get; init; } = new();

    public IReadOnlySet<int> CompanyIds { get; private set; } = new HashSet<int>();
}

public class NotImmutableExample
{
    public List<int> Ints { get; set; } = new();
}

[Immutable]
public class ImmutableExample
{
    public Person Person { get; init; } = new();
    public NotImmutableExample MutableExample { get; init; } = new();
}