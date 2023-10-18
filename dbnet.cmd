@echo off

REM Run
dotnet run --project "./src/dbnet/" --verbosity quiet -- %* || exit /b
