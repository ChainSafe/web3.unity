#!/bin/bash
set -e 

edit=""
if [ "$1" == "ci" ]; then edit="--verify-no-changes"; fi

dotnet format --verbosity=d $edit --severity=warn ./ChainSafe.Gaming.sln

pushd src/UnitySampleProject
dotnet format --verbosity=d $edit --severity=warn ./UnitySampleProject.sln
popd