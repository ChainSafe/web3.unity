@echo off

SET SCRIPT_DIR=%~dp0

REM Change to the directory where the script resides, then navigate to the parent.
pushd "%SCRIPT_DIR%\.."

REM clone submodules
git submodule update --init

REM publish DLLs to unity package
"%SCRIPT_DIR%\publish-to-unity-package.bat"

popd
