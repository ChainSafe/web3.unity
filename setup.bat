@echo off

REM clone submodules
git submodule update --init

REM publish DLLs to unity package
pushd src\ChainSafe.Gaming.Unity
.\publish-to-unity-package.bat
popd