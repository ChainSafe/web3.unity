@echo off

SET SCRIPT_DIR=%~dp0

setlocal enabledelayedexpansion

echo Building project...
pushd "%SCRIPT_DIR%\..\src\ChainSafe.Gaming.Unity"

rem Publish the project
dotnet publish ChainSafe.Gaming.Unity.csproj -c Debug /property:Unity=true

IF %ERRORLEVEL% NEQ 0 (
    echo Execution failed
    exit /b %ERRORLEVEL%
)

set PUBLISH_PATH=bin\Debug\netstandard2.1\publish

rem List generated DLLs
echo DLLs Generated
dir /b "%PUBLISH_PATH%"

set PACKAGE_LIB_PATH=

rem Read and process each line from the dependencies file
for /f "usebackq tokens=*" %%A in ("%SCRIPT_DIR%\data\published_dependencies.txt") do (
    
    set entry=%%A
    
    rem Check if the line ends with a colon
    if "!entry:~-1!" == ":" (
        set "PACKAGE_LIB_PATH=%SCRIPT_DIR%..\!entry:~0,-1!"
        if exist "!PACKAGE_LIB_PATH!\" (
            del /q "!PACKAGE_LIB_PATH!\*.dll"
            del /q "!PACKAGE_LIB_PATH!\*.pdb"
        ) else (
            mkdir "!PACKAGE_LIB_PATH!"
        )
        
        echo Copying to !PACKAGE_LIB_PATH!...
    ) else (
        set "DEPENDENCY=!entry: =!"
        copy /y "%PUBLISH_PATH%\!DEPENDENCY!.dll" "!PACKAGE_LIB_PATH!"
        copy /y "%PUBLISH_PATH%\!DEPENDENCY!.pdb" "!PACKAGE_LIB_PATH!"
    )
)

popd

rem Restore solution
pushd "..\"
dotnet restore
popd

echo Done