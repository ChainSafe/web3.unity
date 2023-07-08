@echo off

REM clone submodules
git submodule update --init

REM publish DLLs to unity package
pushd src\ChainSafe.GamingSdk.EVM.Unity
.\publish-to-unity-package.bat
popd