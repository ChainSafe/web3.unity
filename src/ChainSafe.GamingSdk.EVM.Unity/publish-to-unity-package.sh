#! /usr/bin/env sh

set -e

echo Building project . . .

rm -rf obj
rm -rf bin
dotnet publish -c release -f netstandard2.1 /property:Unity=true

echo Moving files to Unity package . . .

cd bin/release/netstandard2.1/publish
rm Newtonsoft.Json.dll
rm UnityEngine.dll
mkdir -p ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries
rm -f ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries/*
cp *.dll ../../../../../UnityPackages/io.chainsafe.web3-unity/Runtime/Libraries

echo Done
