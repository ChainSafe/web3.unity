#! /usr/bin/env sh

./scripts/setup.sh

git add ./Packages/io.chainsafe.web3-unity.web3auth/.

MESSAGE="release-v$1"

git commit -m "$MESSAGE"
git tag -a "io.chainsafe.web3-unity.web3auth/$1" -m "$MESSAGE"
git push