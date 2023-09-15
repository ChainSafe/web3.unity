#! /usr/bin/env sh

./setup.sh

git add ./Packages/io.chainsafe.web3-unity/.

git add ./Packages/io.chainsafe.web3-unity/Runtime/Libraries/. -f

MESSAGE="release-v$1"

git commit -m "$MESSAGE"
git tag -a "io.chainsafe.web3-unity/$1" -m "$MESSAGE"
git push