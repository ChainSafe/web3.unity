@echo off

dotnet format --verbosity=d --verify-no-changes --severity=warn .\ChainSafe.Gaming.sln --exclude .\submodules

pushd src/UnitySampleProject
dotnet format --verbosity=d --verify-no-changes --severity=warn .\UnitySampleProject.sln
popd 