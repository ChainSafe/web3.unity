#! /usr/bin/env sh
scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

pushd "$scripts_dir"/../
# clone submodules
git submodule update --init
popd

## if directory exists, we make check and cd here so script can work from both when bash is run from scripts and from repo root
[ -d "scripts" ] && cd scripts

# publish DLLs to unity package
source "$scripts_dir"/publish-to-unity-package.sh

