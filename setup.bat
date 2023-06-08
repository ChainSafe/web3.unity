@echo off

REM make sure submodules are up to date
git submodule update --init

REM restore packages
dotnet tool restore
dotnet paket restore
