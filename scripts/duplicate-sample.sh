#! /usr/bin/env sh
scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

pushd "$scripts_dir"/../

export SOURCE_DIRECTORY="$1"
export DESTINATION_DIRECTORY="$2"
rm -r "$DESTINATION_DIRECTORY"
cp -r "$SOURCE_DIRECTORY" "$DESTINATION_DIRECTORY"