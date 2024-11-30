# Immutable Analyzer
[![Tests](https://github.com/protomorphine/ImmutableAnalyzer/actions/workflows/test-report.yaml/badge.svg)](https://github.com/protomorphine/ImmutableAnalyzer/runs/30222165790)
[![Docs](https://github.com/protomorphine/ImmutableAnalyzer/actions/workflows/docs.yml/badge.svg?branch=master)](https://github.com/protomorphine/ImmutableAnalyzer/actions/workflows/docs.yml)

This project is a .NET analyzer tool that utilizes Roslyn to check a class for immutability based on
the types of it's properties and their accessor list.

## Overview

Immutability is a key concept in functional programming and can help  prevent bugs related to mutable state.   
The purpose of this tool is to help developers ensure that their classes follow immutability principles,
which can lead to more stable and predictable code.

## How To Use
<div class="tabbed">

- <b class="tab-title">Manually</b>
    Get the sources of analyzer projects
    ```bash
    $ git clone https://github.com/protomorphine/ImmutableAnalyzer
    ```
    Add project reference to your `.csproj` file:
    ```xml
    <ItemGroup>
        <ProjectReference Include="../ImmutableAnalyzer.Attributes/ImmutableAnalyzer.Attributes.csproj" />
        <ProjectReference Include="../ImmutableAnalyzer.CodeFixes/ImmutableAnalyzer.CodeFixes.csproj" />
        <ProjectReference Include="../ImmutableAnalyzer/ImmutableAnalyzer.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
    </ItemGroup>
    ```
- <b class="tab-title">Nuget</b>
    At this moment project **NOT** published on [NuGet](https://nuget.org/). Stay tuned.

</div>

## Documentation
Made with:
- [Doxygen](https://www.doxygen.nl/)
- [doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css)
