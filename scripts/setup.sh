#! /usr/bin/env sh
scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

pushd "$scripts_dir"/../
# clone submodules
git submodule update --init
popd

# publish DLLs to unity package
source "$scripts_dir"/publish-to-unity-package.sh

