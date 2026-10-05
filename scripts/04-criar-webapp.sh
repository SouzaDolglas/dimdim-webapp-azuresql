#!/usr/bin/env bash
# Cria o App Service Plan + Web App (Linux, .NET) e injeta as configurações:
# string de conexão do Azure SQL e do Application Insights (nada disso fica no código)
set -euo pipefail
source "$(dirname "$0")/00-variaveis.sh"
: "${SQL_PASSWORD:?exporte SQL_PASSWORD antes de rodar}"

az appservice plan create \
  --name "$PLAN" --resource-group "$RG" --location "$LOCATION" \
  --sku "$SKU" --is-linux --output table

az webapp create \
  --name "$WEBAPP" --resource-group "$RG" --plan "$PLAN" \
  --runtime "$RUNTIME" --output table

# Conexão com o Application Insights
AI_CONN=$(az monitor app-insights component show \
  --app "$APPINSIGHTS" --resource-group "$RG" --query connectionString -o tsv)

az webapp config appsettings set \
  --name "$WEBAPP" --resource-group "$RG" \
  --settings APPLICATIONINSIGHTS_CONNECTION_STRING="$AI_CONN" \
  --output none

# Conexão com o Azure SQL (vira ConnectionStrings:DimDim dentro da aplicação)
SQL_CONN="Server=tcp:${SQL_SERVER}.database.windows.net,1433;Database=${SQL_DB};User ID=${SQL_ADMIN};Password=${SQL_PASSWORD};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

az webapp config connection-string set \
  --name "$WEBAPP" --resource-group "$RG" \
  --connection-string-type SQLAzure \
  --settings DimDim="$SQL_CONN" \
  --output none

az webapp config set --name "$WEBAPP" --resource-group "$RG" --always-on true --output none

echo "Web App criado: https://${WEBAPP}.azurewebsites.net"
