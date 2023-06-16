dotnet publish -c release /property:Unity=true

pushd ..\..
dotnet restore
popd