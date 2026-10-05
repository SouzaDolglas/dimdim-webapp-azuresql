#!/usr/bin/env bash
# Cria o Log Analytics + Application Insights que vão monitorar o Web App
set -euo pipefail
source "$(dirname "$0")/00-variaveis.sh"

az extension add --name application-insights --upgrade --only-show-errors

az monitor log-analytics workspace create \
  --resource-group "$RG" --workspace-name "$LAW" --location "$LOCATION" \
  --output table

az monitor app-insights component create \
  --app "$APPINSIGHTS" --resource-group "$RG" --location "$LOCATION" \
  --kind web --application-type web --workspace "$LAW" \
  --output table
