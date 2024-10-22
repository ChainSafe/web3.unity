#! /usr/bin/env sh
scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

# clone submodules
git submodule update --init

pushd "$scripts_dir"/../Setup

# publish DLLs to unity package
dotnet run -sync_dependencies -git:disabled -c ${1:-Release} Setup.csproj

popd