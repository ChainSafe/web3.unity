#! /usr/bin/env sh

echo Building project . . .

dotnet publish -c release -f netstandard2.1 /property:Unity=true

echo Moving files to Unity package . . .

cd bin/release/netstandard2.1/publish
rm Newtonsoft.Json.dll
rm UnityEngine.dll
rm -f ../../../../../UnityPackage/Assets/Plugins/Web3/Libraries/*
cp *.dll ../../../../../UnityPackage/Assets/Plugins/Web3/Libraries

echo Done
