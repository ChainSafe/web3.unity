@ECHO OFF

echo Building project . . .

dotnet publish -c release -f netstandard2.1 /property:Unity=true
if %errorlevel% neq 0 exit /b %errorlevel%

echo Moving files to Unity package . . .

cd bin\release\netstandard2.1\publish
del Newtonsoft.Json.dll
del UnityEngine.dll
if not exist ..\..\..\..\..\UnityPackage\Assets\Plugins\Web3\Libraries mkdir ..\..\..\..\..\UnityPackage\Assets\Plugins\Web3\Libraries
del ..\..\..\..\..\UnityPackage\Assets\Plugins\Web3\Libraries\* /F /Q
copy *.dll ..\..\..\..\..\UnityPackage\Assets\Plugins\Web3\Libraries

echo Done
