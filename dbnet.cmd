@echo off

REM Restore + Build
dotnet build "./src/dbnet" --nologo || exit /b

REM Run
dotnet run --project "./src/dbnet/" --no-build -- %* || exit /b
