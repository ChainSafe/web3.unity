#! /usr/bin/env sh
set -e

## if directory exists, we make check and cd here so script can work from both when bash is run from scripts and from repo root
[ -d "scripts" ] && cd scripts

echo Building project...
pushd ../src/ChainSafe.Gaming.Unity

rm -rf obj
rm -rf bin
dotnet publish -c release -f netstandard2.1 /property:Unity=true

echo Restoring non-Unity packages...

dotnet restore

echo Moving files to Unity package...

pushd bin/release/netstandard2.1/publish
rm Newtonsoft.Json.dll
rm UnityEngine.dll
rm Microsoft.CSharp.dll
rm -rf ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
mkdir -p ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
cp *.dll ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
popd
popd
echo Done
