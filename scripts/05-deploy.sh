#!/usr/bin/env bash
# Compila a aplicação, gera o pacote .zip e faz o deploy com az webapp deploy
set -euo pipefail
source "$(dirname "$0")/00-variaveis.sh"

RAIZ="$(cd "$(dirname "$0")/.." && pwd)"
SAIDA="$RAIZ/publish"

rm -rf "$SAIDA" "$RAIZ/app.zip"
dotnet publish "$RAIZ/src/DimDim.Web/DimDim.Web.csproj" -c Release -o "$SAIDA"
(cd "$SAIDA" && zip -qr "$RAIZ/app.zip" .)

az webapp deploy \
  --name "$WEBAPP" --resource-group "$RG" \
  --src-path "$RAIZ/app.zip" --type zip

echo "Aplicação publicada: https://${WEBAPP}.azurewebsites.net"
