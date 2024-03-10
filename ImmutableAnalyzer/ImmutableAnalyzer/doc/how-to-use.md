# How To Use

## From source

Create reference to project, containing analyzers, in your `.csproj` file.
```xml
    <ItemGroup>
        <ProjectReference
            Include="..\ImmutableAnalyzer\ImmutableAnalyzer.csproj"
            OutputItemType="Analyzer"
            ReferenceOutputAssembly="true" />
    </ItemGroup>
```
Mark classes, that you would like to be immutable, with special `[Immutable]` attribute.

<div class="section_buttons">

| Previous                   | Next |
|:---------------------------|-----:|
| [Home](../../../README.md) |      |

</div>