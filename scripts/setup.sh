#! /usr/bin/env sh

# clone submodules
git submodule update --init

# publish DLLs to unity package
./scripts/publish-to-unity-package.sh
