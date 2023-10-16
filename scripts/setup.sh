#! /usr/bin/env sh

# clone submodules
git submodule update --init

## if directory exists, we make check and cd here so script can work from both when bash is run from scripts and from repo root
[ -d "scripts" ] && cd scripts

# publish DLLs to unity package
./publish-to-unity-package.sh
