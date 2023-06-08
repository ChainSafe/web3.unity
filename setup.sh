#! /usr/bin/env sh

# make sure submodules are up to date
git submodule update --init

# restore packages
dotnet tool restore
dotnet paket restore
