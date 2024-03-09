# Example with enums {#enum-sample}

Enums are immutable by default.

```csharp
public enum PetType
{
    Dog,
    Cat,
    Parrot
}

[Immutable]
public class Pet
{
    public string Name { get; }
    public PetType PetType { get; init; }
}
```

<div class="section_buttons">

| Previous                | Next |
|:------------------------|-----:|
| @ref user-defined-types |      |

</div>