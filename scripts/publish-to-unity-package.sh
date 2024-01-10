#! /usr/bin/env sh
set -e

echo Building project...
scripts_dir=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )

pushd "$scripts_dir"/../src/ChainSafe.Gaming.Unity

rm -rf obj
rm -rf bin
dotnet publish -c release -f netstandard2.1 /property:Unity=true

echo Restoring non-Unity packages...

dotnet restore

echo Moving files to Unity package...

export PUBLISH_PATH="bin/Release/netstandard2.1/publish"

echo -e "DLLs Generated\n$(ls "$PUBLISH_PATH")"

export PACKAGE_DEPENDENCIES=($(<$scripts_dir/data/published_dependencies.txt))

PACKAGE_DEPENDENCIES="${PACKAGE_DEPENDENCIES//$'\n'/ }"
PACKAGE_DEPENDENCIES="${PACKAGE_DEPENDENCIES//$'\r'/}"

for entry in "${PACKAGE_DEPENDENCIES[@]}"
do
  IFS=':' read -ra dirs <<< "$entry"
  
  export PACKAGE_LIB_PATH=$scripts_dir/../${dirs[0]}
  
  if [ -d "$PACKAGE_LIB_PATH" ]; then
    rm -f "$PACKAGE_LIB_PATH"*.dll
  else
    mkdir -p "$PACKAGE_LIB_PATH"
  fi
  
  IFS=';' read -ra dependencies <<< "${dirs[1]}"
  
  for dependency in "${dependencies[@]}"
  do
    cp "$PUBLISH_PATH/$dependency.dll" $PACKAGE_LIB_PATH
  done
done

popd

echo Done
