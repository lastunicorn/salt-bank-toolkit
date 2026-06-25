# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build ./SaltBank.Toolkit.slnx -c Release

# Run all tests
dotnet test ./SaltBank.Toolkit.slnx

# Run tests for a specific project
dotnet test sources/SaltBank.Toolkit.Tests/SaltBank.Toolkit.Tests.csproj

# Run a single test by name
dotnet test sources/SaltBank.Toolkit.Tests/SaltBank.Toolkit.Tests.csproj --filter "FullyQualifiedName~WhenCsvHasSingleRow"

# Restore packages (uses local nuget.config)
dotnet restore ./SaltBank.Toolkit.slnx --configfile ./nuget.config
```

## Architecture

This is a .NET 8 class library published as NuGet package `DustInTheWind.SaltBank.Toolkit`. It parses CSV bank statement files exported from Salt Bank (Romanian bank).

**Projects:**
- `sources/SaltBank.Toolkit` — public library (net8.0), produces the NuGet package
- `sources/SaltBank.Toolkit.Tests` — xUnit tests (net10.0)
- `sources/SaltBank.Toolkit.Demo` — usage example
- `sources/TestingToolkit` — shared test helpers

Assembly names are prefixed with `DustInTheWind.` via `Directory.Build.props` using `AssemblyName>DustInTheWind.$(MSBuildProjectName)`.

**Public surface of `SaltBank.Toolkit`:**
- `StatementDocument` — entry point; extends `Collection<BankTransaction>`. All `Load*` methods are static async and accept file path, string CSV, `Stream`, `FileInfo`, `StreamReader`, or `TextReader`.
- `BankTransaction` — plain data class mapping each CSV column.
- `SpendingCategory` / `TransactionType` — sealed record classes with well-known static instances (e.g. `SpendingCategory.Groceries`) and implicit conversions to/from `string`. Both expose a `KnownValues` collection.
- Exception hierarchy: `DocumentLoadException` (base) → `HeaderLoadException`, `DataLoadException`.

**Internal CSV layer (`Csv/`):**
- `StatementCsvDocument` wraps CsvHelper and drives a two-step read: `ReadHeaderRowAsync()` first (extracts currency from column name like `Amount (RON)`), then `ReadTransactionsAsync()`.
- `StatementCsvHeader` detects currency from the amount column header.
- `BankTransactionMap` is a CsvHelper `ClassMap` that wires columns to properties; it uses currency-aware column names.
- `SpendingCategoryConverter` / `TransactionTypeConverter` handle CSV ↔ record-class conversion.

## Formatting

- **Indentation:** tabs everywhere. Never use spaces for indentation. Enforced via `.editorconfig` and `.DotSettings`.
- **Alignment:** do not align tokens across lines with extra spaces or tabs. Indentation only — no visual alignment of parameters, assignments, or comments.
- **Line endings:** LF (`\n`) in all files. Enforced via `.gitattributes` — git normalizes to LF on commit regardless of platform.
- Exception: `.yml`/`.yaml` files use 2-space indentation because the YAML spec forbids tabs.

## Code Conventions

- No `var` — always use the explicit type.
- LINQ lambda parameter: use `x` for the item.
- Prefer `new()` target-typed object creation.
- Object initializers with more than one property: each property on its own line.
- No curly braces for single-line `if`, `for`, or `using` bodies.
- No underscore prefix on private fields.

## XML Documentation

Only add XML doc comments to types/members that are part of the public NuGet package API. Internal and test types get no XML docs.

## Tests

Framework: xUnit + FluentAssertions (both globally imported in the test project).

**Structure:** One file per tested method, grouped in a directory named after the class under test.
- `sources/SaltBank.Toolkit.Tests/StatementDocumentTests/Load_CsvTests.cs`
- `sources/SaltBank.Toolkit.Tests/StatementDocumentTests/Load_TextReaderTests.cs`

**Naming pattern:** `Having<setup>_When<action>_Then<expectation>`

**`Assert.Throws` / exception assertions:** Always use a block body for the lambda:
```csharp
action.Should().Throw<DocumentLoadException>()
    .WithInnerException<IOException>();
```

**Test resources:** CSV fixtures are embedded resources placed in a `<TestFile>Tests.resources/` subfolder and loaded via `TestResources.GetEmbeddedResourceAsText(FileExtension.Csv)` (the helper resolves the calling test method name to the matching `.csv` file automatically).
