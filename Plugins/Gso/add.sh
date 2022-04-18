#!/bin/zsh
PLUGIN="${1:-1}"
#dotnet clean ./Plugin${PLUGIN}
dotnet build ./Plugin${PLUGIN}
cp ./Plugin${PLUGIN}/bin/Debug/net6.0/Lis.Gso.Plugin${PLUGIN}.dll ../../Lis/Gso/Server/wwwroot/plugins/Lis.Gso.Plugin${PLUGIN}.dll
touch ../../Lis/Gso/Server/Program.cs
