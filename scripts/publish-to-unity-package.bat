

SET SCRIPT_DIR=%~dp0

echo Building project...
pushd "%SCRIPT_DIR%\..\src\ChainSafe.Gaming.Unity"

del obj /F /Q
del bin /F /Q
dotnet restore
dotnet publish -c release -f netstandard2.1 /property:Unity=true
if %errorlevel% neq 0 exit /b %errorlevel%

echo Moving files to Unity package...

echo Moving files to Unity package...

set "PUBLISH_PATH=bin\Release\netstandard2.1\publish"

echo DLLs Generated
dir /b "%PUBLISH_PATH%"

setlocal enabledelayedexpansion
set "PACKAGE_DEPENDENCIES_FILE=%SCRIPT_DIR%\data\published_dependencies.txt"

for /f "tokens=1,* delims=:" %%a in (%PACKAGE_DEPENDENCIES_FILE%) do (
    set "PACKAGE_LIB_PATH=%SCRIPT_DIR%\..\%%a"
    
    if exist "!PACKAGE_LIB_PATH!" (
        del /q "!PACKAGE_LIB_PATH!\*.dll"
    ) else (
        mkdir "!PACKAGE_LIB_PATH!"
    )
    
    for %%d in (%%b) do (
        copy "%PUBLISH_PATH%\%%d.dll" "!PACKAGE_LIB_PATH!"
    )
)

popd

echo Done
