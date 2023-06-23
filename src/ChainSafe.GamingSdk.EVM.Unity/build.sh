#! /usr/bin/env sh
dotnet build /property:Unity=true

pushd ../..
dotnet restore
popd