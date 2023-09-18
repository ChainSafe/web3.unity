#! /usr/bin/env sh

# clone submodules
git submodule update --init

# publish DLLs to unity package
pushd src/ChainSafe.Gaming.Unity
./publish-to-unity-package.sh
popd
