@ECHO OFF

echo Building project...

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
if not exist ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries mkdir ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries\
del ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries\* /F /Q
copy *.dll ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries
copy *.pdb ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries

popd

echo Done
