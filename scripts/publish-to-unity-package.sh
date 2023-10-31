#!/usr/bin/env sh
set -e

echo "Building project..."
scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

pushd "$scripts_dir"/../src/ChainSafe.Gaming.Unity

rm -rf obj
rm -rf bin
dotnet publish -c release -f netstandard2.1 /property:Unity=true

echo "Restoring non-Unity packages..."
dotnet restore

echo "Moving files to Unity package..."

pushd bin/release/netstandard2.1/publish
rm Newtonsoft.Json.dll
rm UnityEngine.dll

# Check if io.chainsafe.web3-unity.lootboxes directory exists
if [ -d "../../../../../../Packages/io.chainsafe.web3-unity.lootboxes" ]; then
    rm -rf ../../../../../../Packages/io.chainsafe.web3-unity.lootboxes/Chainlink/Runtime/Libraries
    mkdir -p ../../../../../../Packages/io.chainsafe.web3-unity.lootboxes/Chainlink/Runtime/Libraries
    cp Chainsafe.Gaming.Chainlink.dll ../../../../../../Packages/io.chainsafe.web3-unity.lootboxes/Chainlink/Runtime/Libraries
    cp Chainsafe.Gaming.LootBoxes.Chainlink.dll ../../../../../../Packages/io.chainsafe.web3-unity.lootboxes/Chainlink/Runtime/Libraries
fi

# Delete those DLLs so they don't get copied in the next step
rm Chainsafe.Gaming.Chainlink.dll
rm Chainsafe.Gaming.LootBoxes.Chainlink.dll

rm Microsoft.CSharp.dll

# Check if BouncyCastle.Crypto.dll exists in the target directory and back it up if it does
if [ -e ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries/BouncyCastle.Crypto.dll ]; then
    cp ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries/BouncyCastle.Crypto.dll ../../../../../../Packages/io.chainsafe.web3-unity/
fi

rm -rf ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
mkdir -p ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
cp *.dll ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries

# Restore the backed-up BouncyCastle.Crypto.dll if it exists
if [ -e ../../../../../../Packages/io.chainsafe.web3-unity/BouncyCastle.Crypto.dll ]; then
    mv ../../../../../../Packages/io.chainsafe.web3-unity/BouncyCastle.Crypto.dll ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
fi

popd
popd
echo "Done"
