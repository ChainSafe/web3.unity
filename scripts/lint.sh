#!/bin/bash
set -e 

edit=""
if [ "$1" == "ci" ]; then edit="--verify-no-changes"; fi

dotnet format --verbosity=d $edit --severity=warn ./ChainSafe.Gaming.sln --exclude ./submodules

if [ "$edit" == "" ]; then
    echo "Linting Unity Sample Project"
    pushd src/UnitySampleProject
    dotnet format --verbosity=d --severity=warn ./UnitySampleProject.sln 
    popd
fi
