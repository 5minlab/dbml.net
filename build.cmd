@echo off

REM Restore dotnet tools
dotnet tool restore

REM Restore + Build
dotnet build --nologo || exit /b

REM Test + Coverage collect
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura --nologo --no-build

REM Remove previous Coverage Report.
del "coveragereport/*"

REM Generate Coverage Report
reportgenerator "-reports:tests/**/coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
