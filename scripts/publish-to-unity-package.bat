

SET SCRIPT_DIR=%~dp0

echo Building project...
pushd "%SCRIPT_DIR%\..\src\ChainSafe.Gaming.Unity"

del obj /F /Q
del bin /F /Q
dotnet restore
dotnet publish -c release -f netstandard2.1 /property:Unity=true
if %errorlevel% neq 0 exit /b %errorlevel%

echo Restoring non-Unity packages...

echo Moving files to Unity package...

pushd bin\release\netstandard2.1\publish
del Newtonsoft.Json.dll
del UnityEngine.dll

if exist "..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes" (
    echo Directory exists, performing actions...
    rmdir /s /q "..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Chainlink\Runtime\Libraries"
    mkdir "..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Chainlink\Runtime\Libraries"
    copy Chainsafe.Gaming.Chainlink.dll "..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Chainlink\Runtime\Libraries"
    copy Chainsafe.Gaming.LootBoxes.Chainlink.dll "..\..\..\..\..\..\Packages\io.chainsafe.web3-unity.lootboxes\Chainlink\Runtime\Libraries"
) else (
    echo Directory does not exist, skipping actions.
)

del Chainsafe.Gaming.Chainlink.dll
del Chainsafe.Gaming.LootBoxes.Chainlink.dll

del Microsoft.CSharp.dll
if not exist ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries mkdir ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries\
del ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries\* /F /Q
copy *.dll ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries
popd
popd

echo Done
