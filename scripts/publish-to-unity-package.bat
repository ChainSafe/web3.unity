@ECHO OFF

echo Building project...
pushd ..\src\ChainSafe.Gaming.Unity

del /S /Q obj
del /S /Q bin
dotnet publish -c release -f netstandard2.1 /property:Unity=true
if %errorlevel% neq 0 exit /b %errorlevel%

echo Restoring non-Unity packages...

pushd ..\..
dotnet restore
popd

echo Moving files to Unity package...

pushd bin\release\netstandard2.1\publish
del Newtonsoft.Json.dll
del UnityEngine.dll

REM Check if io.chainsafe.web3-unity.lootboxes directory exists
if exist ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes (
    del /Q ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Runtime\Libraries\*
    mkdir ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Runtime\Libraries\
    copy Chainsafe.Gaming.Chainlink.dll ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Runtime\Libraries\
    copy Chainsafe.Gaming.Chainlink.LootBoxes.dll ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Runtime\Libraries\
    
)
REM Delete those DLLs so they don't get copied in the next step
del Chainsafe.Gaming.Chainlink.dll
del Chainsafe.Gaming.Chainlink.LootBoxes.dll

if not exist ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries (
    mkdir ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries\
)
del /Q ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries\*
copy *.dll ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries\

popd
popd
echo Done
