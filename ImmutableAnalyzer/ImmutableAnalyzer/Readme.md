# Immutable Analyzer

A set of two Roslyn Analyzers to check class immutability on compile time.   
Immutable classes must be marked with [`[Immutable]`](ImmutableAttribute.cs) attribute.

### Analyzers

- [ImmutablePropertyTypeAnalyzer.cs](Analyzers/ImmutablePropertyTypeAnalyzer.cs): An analyzer that checks the mutable properties in immutable classes.
- [ImmutableSetAccessorAnalyzer.cs](Analyzers/ImmutableSetAccessorAnalyzer.cs): An analyzer that reports the public set accessor of properties in immutable classes.

## How To?
### How to debug?
- Use the [launchSettings.json](Properties/launchSettings.json) profile.
- Debug tests.

### How to use?
See [How To Use](doc/how-to-use.md) document.

### Learn more about wiring analyzers
The complete set of information is available at [roslyn github repo wiki](https://github.com/dotnet/roslyn/blob/main/docs/wiki/README.md).
