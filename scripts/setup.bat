@echo off

REM clone submodules
git submodule update --init

REM publish DLLs to unity package
.\publish-to-unity-package.bat
