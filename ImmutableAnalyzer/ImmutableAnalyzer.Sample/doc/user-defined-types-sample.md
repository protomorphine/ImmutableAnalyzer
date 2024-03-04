# Example with user defined types {#user-defined-types}

If class, marked by `Immutable` attribute, has property with user defined immutable type no diagnostic will reported.

```csharp
[Immutable]
public class Person {}

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

<div class="section_buttons">

| Previous                     |                    Next |
|:-----------------------------|------------------------:|
| [Built-In types](Samples.md) | [Enums](enum-sample.md) |

</div>