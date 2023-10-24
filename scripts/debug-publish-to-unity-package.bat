@ECHO OFF

echo Building project...
pushd src/ChainSafe.Gaming.Unity

del obj /F /Q
del bin /F /Q
dotnet publish -c debug -f netstandard2.1 /property:Unity=true
if %errorlevel% neq 0 exit /b %errorlevel%

echo Restoring non-Unity packages...

pushd ..\..
dotnet restore
popd

echo Moving files to Unity package...

pushd bin\debug\netstandard2.1\publish
del Newtonsoft.Json.dll
del UnityEngine.dll
del Microsoft.CSharp.dll
if not exist ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries mkdir ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries\
del ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries\* /F /Q
copy *.dll ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries
copy *.pdb ..\..\..\..\..\..\Packages\io.chainsafe.web3-unity\Runtime\Libraries

popd
popd
echo Done