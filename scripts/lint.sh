#!/bin/bash
set -e 

SCRIPT_DIR="$(dirname "$0")"

edit=""
if [ "$1" == "ci" ]; then 
    edit="--verify-no-changes"
    dotnet format --verbosity=d $edit --severity=warn ChainSafe.Gaming.sln --exclude ./submodules
else
    pushd "$SCRIPT_DIR/.."
    dotnet format --verbosity=d $edit --severity=warn ChainSafe.Gaming.sln --exclude /submodules
fi

if [ "$edit" == "" ]; then
    echo "Linting Unity Sample Project"
    pushd "$SCRIPT_DIR/../src/UnitySampleProject"
    dotnet format --verbosity=d --severity=warn UnitySampleProject.sln
    popd
    popd
fi
