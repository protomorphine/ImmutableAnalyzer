# Samples {#samples}

<div style="display: none">
@subpage enum-sample
@subpage user-defined-types
</div>

## Example with built-in types

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

<div class="section_buttons">

| Previous                   |                                               Next |
|:---------------------------|---------------------------------------------------:|
| [Home](../../../README.md) | [User defined types](user-defined-types-sample.md) |

</div>


