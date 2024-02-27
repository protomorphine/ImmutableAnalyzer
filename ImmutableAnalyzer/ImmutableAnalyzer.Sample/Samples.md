# Samples

## Example with build in types

```csharp
[Immutable]
public class Person
{
    public long Id { get; set; }

    public List<string> Names { get; init; } = new();

    public IReadOnlySet<int> CompanyIds { get; private set; } = new HashSet<int>();
}
```

In the example above:
- the `Id` property would be considered mutable due to it's public setter;
- the `Names` list would be considered mutable due to it's mutable type;
- the `CompanyIds` set would be considered immutable;

## Example with user defined types

```csharp
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
```

In this example:
- property `Person` of type `Person` - immutable due `[Immutable]` attribute usage;
- property `MutableExample` of type `NotImmutableExample` - mutable;


