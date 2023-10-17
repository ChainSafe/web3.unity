#! /usr/bin/env sh

scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
source "$scripts_dir"/setup.sh

git add "$scripts_dir"/../Packages/io.chainsafe.web3-unity.web3auth/.

MESSAGE="release-v$1"

git commit -m "$MESSAGE"
git tag -a "io.chainsafe.web3-unity.web3auth/$1" -m "$MESSAGE"
git push