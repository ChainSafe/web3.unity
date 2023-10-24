#! /usr/bin/env sh
rm -rf "$2"
echo "$2 directory removed"
mkdir -p "$2"
echo "$2 directory created"
cp -r "$1"* "$2"
echo "$1 contents copied into $2"