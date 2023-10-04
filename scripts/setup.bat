@echo off
pushd ..
REM clone submodules
git submodule update --init

REM publish DLLs to unity package
.\scripts\publish-to-unity-package.bat
popd