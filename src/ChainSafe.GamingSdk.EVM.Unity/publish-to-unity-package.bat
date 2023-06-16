@ECHO OFF

echo Building project . . .

del obj /F /Q
del bin /F /Q
dotnet publish -c release -f netstandard2.1 /property:Unity=true
if %errorlevel% neq 0 exit /b %errorlevel%

echo Moving files to Unity package . . .

cd bin\release\netstandard2.1\publish
del Newtonsoft.Json.dll
del UnityEngine.dll
if not exist ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries mkdir ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries\
del ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries\* /F /Q
copy *.dll ..\..\..\..\..\UnityPackages\io.chainsafe.web3-unity\Runtime\Libraries

echo Done
