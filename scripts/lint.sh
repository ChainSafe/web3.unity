#!/bin/bash
set -e 

scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
edit=""
if [ "$1" == "ci" ]; then edit="--verify-no-changes"; fi

dotnet format --verbosity=d $edit --severity=warn "$scripts_dir"/../ChainSafe.Gaming.sln --exclude ./submodules

if [ "$edit" == "" ]; then
    echo "Linting Unity Sample Project"
    
    pushd "$scripts_dir"/../src/UnitySampleProject
    dotnet format --verbosity=d --severity=warn ./UnitySampleProject.sln 
    popd
fi
