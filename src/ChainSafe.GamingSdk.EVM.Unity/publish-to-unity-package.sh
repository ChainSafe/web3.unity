#! /usr/bin/env sh

set -e

echo Building project...

rm -rf obj
rm -rf bin
dotnet publish -c release -f netstandard2.1 /property:Unity=true

echo Moving files to Unity package...

rm -f bin/release/netstandard2.1/publish/Newtonsoft.Json.dll
rm -f bin/release/netstandard2.1/publish/UnityEngine.dll
mkdir -p ../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries
rm -f ../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries/*

cd bin/release/netstandard2.1/publish

cp *dll ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries

echo Done
