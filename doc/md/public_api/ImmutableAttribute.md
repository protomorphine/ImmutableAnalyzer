# Immutable attribute {#immutable_attr}

- Can be used on: `class | struct | record`
- Required for: [IM0001](@ref im0001), [IM0002](@ref im0002)

This attribute tells analyzer that, marked type should be immutable and it content should be analyzed.

```csharp
[Immutable]
public class Pet
{
	public string Name { get; set; }     // IM0002 will be reported
	public List<Toy> Toys { get; init; } // IM0001 will be reported
}
```