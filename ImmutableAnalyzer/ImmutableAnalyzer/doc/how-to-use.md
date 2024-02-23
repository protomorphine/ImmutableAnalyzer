# How To Use

Create reference to project, containig analyzers, in your `.csproj` file.
```xml
    <ItemGroup>
        <ProjectReference
            Include="..\ImmutableAnalyzer\ImmutableAnalyzer.csproj"
            OutputItemType="Analyzer"
            ReferenceOutputAssembly="true" />
    </ItemGroup>
```
Mark classes, that you would like to be immutable, with special `[Immutable]` attribute.