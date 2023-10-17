#! /usr/bin/env sh
scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

pushd "$scripts_dir"/../
dotnet publish -c release /property:Unity=true

pushd ../..
dotnet restore

popd
popd