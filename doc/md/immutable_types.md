# Immutable types {#immutable_types}

Following list contains types, which allowed to use as property type in immutable type:

- User defined immutable types (marked by [`ImmutableAttribute`](@ref immutable_attr));
- Types from [`ImmutableTypes.txt`](@ref config) additional file;
- Built-in immutable types:
    - Generic:
      - `ImmutableArray<>`;
      - `ImmutableDictionary<,>`;
      - `ImmutableList<>`;
      - `ImmutableHashSet<>`;
      - `ImmutableSortedDictionary<,>`;
      - `ImmutableSortedSet<>`;
      - `ImmutableStack<>`;
      - `ImmutableQueue<>`;
      - `IReadOnlyList<>`;
      - `IReadOnlyCollection<>`;
      - `IReadOnlyDictionary<,>`;
    - NonGeneric:
      - integral types: `byte`, `sbyte`, `short`, `ushort`, `char`, `int`, `uint`, `long`, `ulong`;
      - floating point types: `float`, `double`, `decimal`;
      - `bool`;
      - `string`;
      - enums;
      - `Guid`;
      - `DateTime`;

<div class="section_buttons">

| Previous                                  |                          Next |
|-------------------------------------------|-------------------------------|
| [Available Diagnostics](@ref diagnostics) | [Public API](@ref public_api) |

</div>