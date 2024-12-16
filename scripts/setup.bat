@echo off

SET SCRIPT_DIR=%~dp0

REM Change to the directory where the script resides, then navigate to the parent.
pushd "%SCRIPT_DIR%\..\Setup"

REM clone submodules
git submodule update --init

if "%1"=="" (
    set "config=Release"
) else (
    set "config=%1"
)

dotnet run -s -g false -c %config% Setup.csproj

popd
