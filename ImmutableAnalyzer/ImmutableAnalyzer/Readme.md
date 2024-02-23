# Immutable Analyzer

A set of two Roslyn Analyzers to check class immutability on compile time.   
Immutable classes must be marked with [`[Immutable]`](ImmutableAttribute.cs) attribute.

Inspired by [this](https://habr.com/ru/companies/otus/articles/676680/#comment_24523640) comment on [habr](https://habr.com/).

## Content
### ImmutableAnalyzer
Contains analyzers.

- [ImmutablePropertyTypeAnalyzer.cs](Analyzers/ImmutablePropertyTypeAnalyzer.cs): An analyzer that reports the mutable property types in immutable class.
- [ImmutableSetAccessorAnalyzer.cs](Analyzers/ImmutableSetAccessorAnalyzer.cs): An analyzer that reports the public set accessor of property in immutable class.

### ImmutableAnalyzer.Sample

Contains demo example of immutable analyzers.

### ImmutableAnalyzer.Tests
Unit tests for the immutable analyzers.

## How To?
### How to debug?
- Use the [launchSettings.json](Properties/launchSettings.json) profile.
- Debug tests.

### How to use?
- Create reference to [ImmutableAnalyzer.csproj](ImmutableAnalyzer.csproj) in your `.csproj` file
```xml
    <ItemGroup>
        <ProjectReference Include="..\ImmutableAnalyzer\ImmutableAnalyzer.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="true" />
    </ItemGroup>
```
- Mark classes, that you would like to be immutable, with `[Immutable]` attribute

### Learn more about wiring analyzers
The complete set of information is available at [roslyn github repo wiki](https://github.com/dotnet/roslyn/blob/main/docs/wiki/README.md).