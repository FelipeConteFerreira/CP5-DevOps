#!/usr/bin/env bash
# Deploy automatizado: dotnet publish + az webapp deploy
set -e
source "$(dirname "$0")/00-variaveis.sh"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
rm -rf "$ROOT/publish" "$ROOT/app.zip"
dotnet publish "$ROOT/src/DimDim/DimDim.csproj" -c Release -o "$ROOT/publish"
(cd "$ROOT/publish" && zip -qr "$ROOT/app.zip" .)
az webapp deploy -g "$RG" -n "$APP" --src-path "$ROOT/app.zip" --type zip
echo "Deploy concluído: https://$APP.azurewebsites.net"
