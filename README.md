# dbml.net <!-- omit in toc -->

[![Build main](https://github.com/Catalin-Andronie/dbml.net/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/Catalin-Andronie/dbml.net/actions/workflows/build.yml)

> Inspired by <https://github.com/holistics/dbml> and elevate to .NET folks!

Abbreviations:

- **DBML**: Database Markup Language

This repo contains:

1. The **dbml.net** or **DbmlNet** library. A parser library build on **.NET** for parsing any **\*.dbml** files.
2. The **dbnet** CLI tool. A **.NET** CLI tool which facilitates generation of a *ready to use* **.sql** file, base on the provided **.dbml** input files.

**Table of contents:**

- [DbmlNet parser features](#dbmlnet-parser-features)
- [Prerequisites](#prerequisites)
- [Running the solution](#running-the-solution)
- [Build solution](#build-solution)
- [Unit tests](#unit-tests)
- [Integration tests](#integration-tests)
- [Code Coverage](#code-coverage)
- [Other commands](#other-commands)

## DbmlNet parser features

Features:

**DbmlNet** features:

- [X] [Project Definition](https://dbml.dbdiagram.io/docs/#project-definition)
- [X] [Schema Definition](https://dbml.dbdiagram.io/docs/#schema-definition)
- [X] [Public Schema](https://dbml.dbdiagram.io/docs/#public-schema)
- [ ] [Table Definition](https://dbml.dbdiagram.io/docs/#table-definition)
  - [ ] [Table Alias](https://dbml.dbdiagram.io/docs/#table-alias)
    - [ ] You can alias the table, and use them in the references later on.
  - [X] [Table Notes](https://dbml.dbdiagram.io/docs/#table-notes)
  - [X] [Table Settings](https://dbml.dbdiagram.io/docs/#table-settings)
- [X] [Column Definition](https://dbml.dbdiagram.io/docs/#column-definition)
  - [X] [Column Settings](https://dbml.dbdiagram.io/docs/#column-settings)
  - [X] [Default Value](https://dbml.dbdiagram.io/docs/#default-value)
- [X] [Index Definition](https://dbml.dbdiagram.io/docs/#index-definition)
  - [X] [Index Settings](https://dbml.dbdiagram.io/docs/#index-settings)
- [X] [Relationships & Foreign Key Definitions](https://dbml.dbdiagram.io/docs/#relationships-foreign-key-definitions)
  - [X] [Relationship settings](https://dbml.dbdiagram.io/docs/#relationship-settings)
  - [X] [Many-to-many relationship](https://dbml.dbdiagram.io/docs/#many-to-many-relationship)
- [ ] [Comments](https://dbml.dbdiagram.io/docs/#comments)
- [X] [Note Definition](https://dbml.dbdiagram.io/docs/#note-definition)
  - [X] [Project Notes](https://dbml.dbdiagram.io/docs/#project-notes)
  - [X] [Table Notes](https://dbml.dbdiagram.io/docs/#table-notes-2)
  - [X] [Column Notes](https://dbml.dbdiagram.io/docs/#column-notes)
- [ ] [Multi-line String](https://dbml.dbdiagram.io/docs/#multi-line-string)
- [ ] [Enum Definition](https://dbml.dbdiagram.io/docs/#enum-definition)
- [ ] [TableGroup](https://dbml.dbdiagram.io/docs/#tablegroup)
- [ ] [Syntax Consistency](https://dbml.dbdiagram.io/docs/#syntax-consistency)

## Prerequisites

01. Install latest [NET Core 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)

02. Check if *dotnet* is installed by running next command:

    ```shell
    dotnet --version
    ```

03. Restore tools used in development by running next command:

    ```shell
    dotnet tool restore
    ```

## Running the solution

The simplest way to run `dbnet` is to use one of the available dbnet.[cmd|sh] scripts and from the `root folder` run next command:

```shell
./dbnet.cmd --help # Windows
./dbnet.sh --help  # Apple and Linux
```

The `--help` option will output the fallowing:

```shell
Usage: dbnet [<file | directory>...] [options]

dbnet helps converting *.dbml syntax to *.sql, *.md syntax. Check the --output-type option.

Arguments:
  <file-or-directory-path> The file or directory path to operate on.

Options:
  --ignore-warnings         Allow files be processed even if the syntax tree contains warnings.
  --print-syntax            Prints the syntax tree.
  --output-type <opt>       Output type to use. Supported values: [sql | markdown]
  -o --output <OUTPUT_DIR>  The output directory to place the artifacts in.
  -h --help                 Show command line help.

Examples:
  dbnet file.dbml   # usage with a file
  dbnet ./dir-name/ # usage with a folder

```

Application requires as `<input>` parameter, a valid path to either a **.dbml** file or a path to a directory which contains at least one **.dbml** file.

Application can be started using one of the next options:

- via *dotnet*, on the `root folder` run next command:

    ```shell
    # usage with a single file
    dotnet run --project ./src/dbnet/ ./samples/appDb.dbml

    # usage with a folder
    dotnet run --project ./src/dbnet/ ./samples/
    ```

- via *project-script*, on the `root folder` run next command:

    ```shell
    # usage with a single file
    ./dbnet.cmd ./samples/appDb.dbml

    # usage with a folder
    ./dbnet.cmd ./samples/
    ```

## Build solution

01. [Check build runs on CI][CI-link] **--or--**

02. On the `root folder` run next command:

    ```shell
    dotnet build
    ```

## Unit tests

01. [Check test runs on CI][CI-link] **--or--**

02. On the `root folder` run next command:

    ```shell
    dotnet cake --task=unit-tests
    ```

## Integration tests

01. [Check test runs on CI][CI-link] **--or--**

02. On the `root folder` run next command:

    ```shell
    dotnet cake --task=integration-tests
    ```

## Code Coverage

01. [Check coverage reports on CI][CI-link] **--or--**

02. On the `root folder` run next command:

    ```shell
    dotnet cake --task=code-coverage
    ```

    - Generated coverage results are open automatically if option `-open-coverage-results` is present:

    ```shell
    dotnet cake --task=code-coverage --open-coverage-results
    ```

## Other commands

- Push commits one by one automatically:

  - Windows (powershell):

    ```powershell
    foreach ($rev in $(git rev-list --reverse origin/branch-name..branch-name))
    {
        git push origin ($rev + ":branch-name") -f
    }
    ```

  - Linux (shell):

    ```shell
    for rev in $(git rev-list --reverse origin/branch-name..branch-name); do
        git push origin $rev:branch-name -f;
    done
    ```

<!-- Links: -->
[CI-link]: https://github.com/Catalin-Andronie/dbml.NET/actions/workflows/build.yml
