#!/bin/bash
set -e 

edit=""
if [ "$1" == "ci" ]; then edit="--verify-no-changes"; fi

if [ "$1" == "ci" ]; then
dotnet format --verbosity=d $edit --severity=warn ChainSafe.Gaming.sln --exclude ./submodules
else
dotnet format --verbosity=d $edit --severity=warn ../ChainSafe.Gaming.sln --exclude ./submodules
fi

if [ "$edit" == "" ]; then
    echo "Linting Unity Sample Project"
    if [ "$1" == "ci" ]; then
    pushd src/UnitySampleProject
    else
    pushd ../src/UnitySampleProject
    fi
    dotnet format --verbosity=d --severity=warn UnitySampleProject.sln
    popd
fi
