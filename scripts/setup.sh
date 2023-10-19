#! /usr/bin/env sh

# Get the directory of the current script
SCRIPT_DIR="$(dirname "$0")"

# clone submodules
git submodule update --init

# publish DLLs to unity package
"$SCRIPT_DIR/publish-to-unity-package.sh"
