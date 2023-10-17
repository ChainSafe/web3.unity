#! /usr/bin/env sh

scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
source "$scripts_dir"/setup.sh

git add "$scripts_dir"/../Packages/io.chainsafe.web3-unity/.

git add "$scripts_dir"/../Packages/io.chainsafe.web3-unity/Runtime/Libraries/. -f

MESSAGE="release-v$1"

git commit -m "$MESSAGE"
git tag -a "io.chainsafe.web3-unity/$1" -m "$MESSAGE"
git push