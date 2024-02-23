# Samples

## Mutable property in immutable class (IM0001)
```csharp
[Immutable]
public class Example
{
    public long Id { get; init; }

    public List<string> Names { get; init; } = new(); // Diagnostic 'IM0001' will be reported on this line

    public IReadOnlySet<int> MyInts { get; private set; } = new HashSet<int>();
}
```

## Public setter in immutable class (IM0002)
```csharp
[Immutable]
public class Example
{
    public long Id { get; set; } // Diagnostic 'IM0002' will be reported on this line

    public IReadOnlyList<string> Names { get; init; } = new List<string>();

    public IReadOnlySet<int> MyInts { get; private set; } = new HashSet<int>();
}
```
