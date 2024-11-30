# Configuration {#config}

Analyzer can be configured by adding additional file, named `ImmutableTypes.txt`.

To specify an individual project item as an additional file, set the item type to `AdditionalFiles` in `csproj` file:

```xml
<ItemGroup>
  <AdditionalFiles Include="ImmutableTypes.txt" />
</ItemGroup>
```

For more options with adding additional files see [Roslyn doc](https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Using%20Additional%20Files.md).

## File content

Each line of file should contain only one type name.

For example

```txt
DateOnly
TimeOnly
```

<div class="section_buttons">

| Previous                      |                                     Next |
|:------------------------------|-----------------------------------------:|
| [Home](../../README.md)    | [Available diagnostics](@ref diagnostics) |

</div>