#! /usr/bin/env sh

set -e

echo Building project...

rm -rf obj
rm -rf bin
dotnet publish -c debug -f netstandard2.1 /property:Unity=true

echo Restoring non-Unity packages...

pushd ../..
dotnet restore
popd

echo Moving files to Unity package...

cd bin/debug/netstandard2.1/publish
rm Newtonsoft.Json.dll
rm UnityEngine.dll
mkdir -p ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries
rm -f ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries/*
cp *.dll ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries
cp *.pdb ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries

echo Done
