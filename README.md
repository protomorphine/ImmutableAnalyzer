# Immutable Analyzer
[![Tests](https://github.com/protomorphine/ImmutableAnalyzer/actions/workflows/test-report.yaml/badge.svg)](https://github.com/protomorphine/ImmutableAnalyzer/runs/22331910530)
[![Docs](https://github.com/protomorphine/ImmutableAnalyzer/actions/workflows/docs.yml/badge.svg?branch=master)](https://github.com/protomorphine/ImmutableAnalyzer/actions/workflows/docs.yml)

This project is a .NET analyzer tool that utilizes Roslyn to check a class for immutability based on
the types of it's properties and their accessor list.

## Overview

Immutability is a key concept in functional programming and can help  prevent bugs related to mutable state.   
The purpose of this tool is to help developers ensure that their classes follow immutability principles,
which can lead to more stable and predictable code.

## Diagnostics
See [Diagnostics](ImmutableAnalyzer/ImmutableAnalyzer/doc/diagnostics.md) page.

## How To Use
See [How To Use](ImmutableAnalyzer/ImmutableAnalyzer/doc/how-to-use.md) page.

## Sample
See [Samples](ImmutableAnalyzer/ImmutableAnalyzer.Sample/doc/Samples.md) page.

## Documentation
Made with:
- [Doxygen](https://www.doxygen.nl/)
- [doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css)
