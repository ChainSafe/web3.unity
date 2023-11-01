#! /usr/bin/env sh

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
rm -rf ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
mkdir -p ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
cp *.dll ../../../../../../Packages/io.chainsafe.web3-unity/Runtime/Libraries
popd
