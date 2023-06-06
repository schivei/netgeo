#!/bin/bash
dotnet build -c Release ./src/NetGeo.Json/NetGeo.Json.csproj
dotnet build -c Release ./src/NetGeo.Newtonsoft.Json/NetGeo.Newtonsoft.Json.csproj
dotnet build -c Release ./src/NetGeo.System.Text.Json/NetGeo.System.Text.Json.csproj

dotnet pack -c Release -o ./build-packages -p:IncludeSymbols=false ./src/NetGeo.Json/NetGeo.Json.csproj
dotnet pack -c Release -o ./build-packages -p:IncludeSymbols=false ./src/NetGeo.Newtonsoft.Json/NetGeo.Newtonsoft.Json.csproj
dotnet pack -c Release -o ./build-packages -p:IncludeSymbols=false ./src/NetGeo.System.Text.Json/NetGeo.System.Text.Json.csproj
