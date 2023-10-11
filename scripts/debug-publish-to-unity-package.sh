#! /usr/bin/env sh

set -e

echo Building project...
pushd src/ChainSafe.Gaming.Unity

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
mkdir -p ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
rm -f ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries/*
cp *.dll ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
cp *.pdb ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries

echo Done
