#! /usr/bin/env sh

set -e

scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

echo Publishing project...

pushd "$scripts_dir"/../src/ChainSafe.Gaming.Unity

dotnet publish ChainSafe.Gaming.Unity.csproj -c Release /property:Unity=true

export PUBLISH_PATH="bin/Release/netstandard2.1/publish"

echo -e "DLLs Generated\n$(ls "$PUBLISH_PATH")"

export PACKAGE_LIB_PATH=

while IFS= read -r entry || [ -n "$entry" ];
do
  entry=$(echo "$entry" | sed -e 's/^[[:space:]]*//' -e 's/[[:space:]]*$//')
  if [[ $entry == *: ]]
  then
    PACKAGE_LIB_PATH="$scripts_dir/../${entry%:}"
    if [ -d "$PACKAGE_LIB_PATH" ]; then
      rm -rf "$PACKAGE_LIB_PATH"*.dll
    else
      mkdir -p "$PACKAGE_LIB_PATH"
    fi
    echo "Copying to $PACKAGE_LIB_PATH..."
  else
    export DEPENDENCY=$(echo "$entry" | tr -d '\t' | tr -d ' ')
    cp -fr "$PUBLISH_PATH/$DEPENDENCY" $PACKAGE_LIB_PATH
  fi
done < "$scripts_dir/data/published_dependencies.txt"

popd

echo Done