#!/usr/bin/env bash
# Cria o Azure SQL Server (PaaS) + banco, libera o firewall e aplica o DDL
set -euo pipefail
source "$(dirname "$0")/00-variaveis.sh"
: "${SQL_PASSWORD:?exporte SQL_PASSWORD antes de rodar}"

az sql server create \
  --name "$SQL_SERVER" --resource-group "$RG" --location "$LOCATION" \
  --admin-user "$SQL_ADMIN" --admin-password "$SQL_PASSWORD" \
  --output table

az sql db create \
  --resource-group "$RG" --server "$SQL_SERVER" --name "$SQL_DB" \
  --edition Basic --capacity 5 --backup-storage-redundancy Local \
  --output table

# Libera os serviços do Azure (o Web App) a acessar o servidor
az sql server firewall-rule create \
  --resource-group "$RG" --server "$SQL_SERVER" \
  --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0 \
  --output table

# Libera o IP da máquina que está rodando os scripts (para aplicar o DDL e consultar)
MEU_IP=$(curl -s https://api.ipify.org)
az sql server firewall-rule create \
  --resource-group "$RG" --server "$SQL_SERVER" \
  --name MinhaMaquina --start-ip-address "$MEU_IP" --end-ip-address "$MEU_IP" \
  --output table

# Cria as tabelas (DDL em scripts/ddl.sql) com o sqlcmd
sqlcmd -S "${SQL_SERVER}.database.windows.net" -d "$SQL_DB" \
  -U "$SQL_ADMIN" -P "$SQL_PASSWORD" -i "$(dirname "$0")/ddl.sql"

sqlcmd -S "${SQL_SERVER}.database.windows.net" -d "$SQL_DB" \
  -U "$SQL_ADMIN" -P "$SQL_PASSWORD" \
  -Q "SELECT name AS tabela FROM sys.tables ORDER BY name"
