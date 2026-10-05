#!/usr/bin/env bash
# =====================================================================
# Projeto DimDim (Web App + Azure SQL) - variáveis usadas por todos os scripts
#
# Uso:  source scripts/00-variaveis.sh
#
# ATENÇÃO: a senha do banco NÃO fica aqui. Exporte antes de rodar os scripts:
#   export SQL_PASSWORD='...'
# =====================================================================

export RM="564099"                        # RM da representante (Natalia)
export RG="rg-${RM}-dimdim-webapp"
export LOCATION="eastus2"                 # regiões liberadas no Azure for Students

# --- Azure SQL ---------------------------------------------------------------
export SQL_SERVER="sql-${RM}-dimdim"      # vira sql-564099-dimdim.database.windows.net
export SQL_DB="dimdim"
export SQL_ADMIN="dimdimadmin"

# --- App Service -------------------------------------------------------------
export PLAN="plan-${RM}-dimdim"
export WEBAPP="app-${RM}-dimdim"          # vira https://app-564099-dimdim.azurewebsites.net
export RUNTIME="DOTNETCORE:10.0"
export SKU="B1"

# --- Monitoramento -----------------------------------------------------------
export LAW="log-${RM}-dimdim"             # Log Analytics workspace
export APPINSIGHTS="appi-${RM}-dimdim"

echo "Variáveis carregadas: RM ${RM}, grupo de recursos ${RG}, região ${LOCATION}"
