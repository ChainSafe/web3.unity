#!/bin/bash

# Specify the source directory and the destination directory
SOURCE_DIRECTORY="src/UnitySampleProject/Assets/Samples/web3.unity SDK/2.5.0-pre001/Web3.Unity Samples/"
DESTINATION_DIRECTORY="src/UnityPackages/io.chainsafe.web3-unity/Samples~/Web3.Unity/"

# clear destination directory first
rm -r "$DESTINATION_DIRECTORY"

# Copy source to the destination
cp -r "$SOURCE_DIRECTORY" "$DESTINATION_DIRECTORY"

#add all modified files
git add "src/UnityPackages/io.chainsafe.web3-unity/Samples~/Web3.Unity/." -f