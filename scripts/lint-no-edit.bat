@echo off

:: Change to the directory where the script resides
pushd %~dp0
:: Navigate to the parent (assuming the script is in a subfolder of the root)
cd ..

:: Run format command in project root
dotnet format --verbosity=d --severity=warn --verify-no-changes ChainSafe.Gaming.sln --exclude /submodules

:: Navigate to the UnitySampleProject within src and run the format command
pushd .\src\UnitySampleProject
dotnet format --verbosity=d --severity=warn --verify-no-changes .\UnitySampleProject.sln
popd 

:: Restore the original directory
popd